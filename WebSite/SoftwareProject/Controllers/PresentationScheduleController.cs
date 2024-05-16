//using GraduationProject.Data.Services;
//using GraduationProject.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GraduationProject.Controllers
//{
//    public class PresentationScheduleController : Controller
//    {
//        private readonly IAvailableDateTimeSlotService _slotService; // Kullanılabilir tarih ve saat slotları servisi
//        private readonly IProfessorService _professorService; // Profesör servisi
//        private readonly IProjectService _projectService; // Proje servisi
//        private const int MaxProfessorsPerProject = 3; // Proje başına maksimum profesör sayısı
//        private readonly IBookedDatesService _bookedDatesService; // Rezervasyon tarihlerine erişim sağlayan servis

//        public PresentationScheduleController(IAvailableDateTimeSlotService slotService, IProfessorService professorService, IProjectService projectService, IBookedDatesService bookedDatesService)
//        {
//            _slotService = slotService; // Slot servisi enjekte ediliyor
//            _professorService = professorService; // Profesör servisi enjekte ediliyor
//            _projectService = projectService; // Proje servisi enjekte ediliyor
//            _bookedDatesService = bookedDatesService; // Rezervasyon tarihleri servisi enjekte ediliyor
//        }

//        public async Task<IActionResult> Index() // Index metodu, sunum programının ana sayfasını oluşturur
//        {
//            var allSlots = await _slotService.GetAllAsync(); // Tüm slotları alır
//            var professors = await _professorService.GetAllAsync(); // Tüm profesörleri alır
//            var projects = await _projectService.GetAllAsync(); // Tüm projeleri alır

//            var presentationSchedule = new Dictionary<Project, List<(DateTime, string, string)>>(); // Sunum takvimini oluşturacak bir dictionary tanımlanır
//            var bookedDates = await _bookedDatesService.GetBookedDatesForProjectsAsync(projects.Select(p => p.Id).ToList()); // Projeler için rezervasyon yapılmış tarihler alınır

//            foreach (var project in projects) // Tüm projeler için döngü
//            {
//                var projectSlots = new List<(DateTime, string, string)>(); // Proje slotlarını tutacak liste oluşturulur
//                var assignedProfessorIds = new HashSet<int>(); // Atanan profesör kimliklerini tutacak küme oluşturulur

//                // Projeyi sunum takvimine ekleyip eklenmediğini kontrol eder
//                //Koleksiyon içerisinde, parametre olarak girilen değerde bir Anahtar (Key) mevcutsa TRUE değilse FALSE döndürecektir.
//                if (!presentationSchedule.ContainsKey(project))
//                {
//                    var professorSlots = allSlots.Where(slot => slot.ProfessorId == project.ProfessorId && slot.isAvailable).ToList(); // Projeye atanmış profesörün boş slotları alınır

//                    // Projenin rezerve edilmiş tarihleri var mı kontrol edilir
//                    if (bookedDates.ContainsKey(project.Id))
//                    {
//                        var projectBookedDates = bookedDates[project.Id]; // Projenin rezerve edilmiş tarihleri alınır
//                        foreach (var bookedDate in projectBookedDates) // Rezerve edilmiş tarihler için döngü
//                        {
//                            var professor = await _professorService.GetByIdAsync(bookedDate.ProfessorId); // Rezerve edilen tarih için ilgili profesör bilgisi alınır
//                            // Projenin rezerve edilmiş tarihlerini ekler
//                            projectSlots.Add((bookedDate.BookedDate.Add(bookedDate.BookedHour), project.StudentID, $"{professor.FirstName} {professor.LastName}"));
//                            //0  yazdırma 
//                        }
//                    }
//                    else // Rezerve edilmiş tarih yoksa
//                    {
//                        // Proje için kullanılabilir slot alınır
//                        var availableSlot = GetAvailableSlot(professorSlots);
//                        if (availableSlot != null)
//                        {
//                            var presentationDateTime = availableSlot.AvailableDate.Add(availableSlot.AvailableHour);
//                            projectSlots.Add((presentationDateTime, project.StudentID, $"{project.Professor.FirstName} {project.Professor.LastName}"));

//                            // availableSlot.isAvailable = false; //her döngüde!!!

//                            // Rezerve edilen tarih veritabanına eklenir
//                            var bookedDatePrimary = new BookedDates
//                            {
//                                ProfessorId = project.ProfessorId,
//                                ProjectId = project.Id,
//                                BookedDate = availableSlot.AvailableDate,
//                                BookedHour = availableSlot.AvailableHour
//                            };
//                            await _bookedDatesService.AddAsync(bookedDatePrimary);

//                            assignedProfessorIds.Add(project.ProfessorId);

//                            foreach (var professor in professors)
//                            {
//                                if (assignedProfessorIds.Count >= MaxProfessorsPerProject)
//                                    break;

//                                if (professor.Id != project.ProfessorId && !assignedProfessorIds.Contains(professor.Id))
//                                {
//                                    // Profesörün aynı zamanda başka bir projeye atanıp atanmadığı kontrol edilir
//                                    //=???!?=!?
//                                    var professorAlreadyAssigned = presentationSchedule.Any(p => p.Value.Any(slot => slot.Item1.Date == availableSlot.AvailableDate.Date && slot.Item1.TimeOfDay == availableSlot.AvailableHour && p.Key.ProfessorId == professor.Id));
//                                    if (professorAlreadyAssigned)
//                                        continue;

//                                    var otherProfessorSlots = allSlots
//                                        .Where(slot => slot.ProfessorId == professor.Id &&
//                                                       slot.AvailableDate.Date == availableSlot.AvailableDate.Date &&
//                                                       slot.isAvailable)
//                                        .ToList();

//                                    var matchingSlot = FindCommonAvailableSlot(professorSlots, otherProfessorSlots);

//                                    if (matchingSlot != null)
//                                    {
//                                        var matchingDateTime = matchingSlot.AvailableDate.Add(matchingSlot.AvailableHour);
//                                        projectSlots.Add((matchingDateTime, project.StudentID, $"{professor.FirstName} {professor.LastName}"));

//                                        // Rezerve edilen tarih veritabanına eklenir
//                                        var bookedDateSecondary = new BookedDates
//                                        {
//                                            ProfessorId = professor.Id,
//                                            ProjectId = project.Id,
//                                            BookedDate = matchingSlot.AvailableDate,
//                                            BookedHour = matchingSlot.AvailableHour
//                                        };
//                                        await _bookedDatesService.AddAsync(bookedDateSecondary);

//                                        assignedProfessorIds.Add(professor.Id);
//                                    }
//                                }
//                            }

//                            // Kullanılan slotlar için isAvailable özelliği false olarak ayarlanır
//                            availableSlot.isAvailable = false;
//                            await _slotService.UpdateAsync(availableSlot.Id, availableSlot);
//                        }
//                    }

//                    presentationSchedule.Add(project, projectSlots); // Sunum takvimine proje ve slotlar eklenir
//                }
//            }

//            return View(presentationSchedule); // Sunum takvimi view'ına döner
//        }

//        private AvailableDateTimeSlot GetAvailableSlot(List<AvailableDateTimeSlot> slots) // Kullanılabilir slotları bulan metot
//        {
//            return slots.OrderBy(s => s.AvailableDate).FirstOrDefault(); // Tarihine göre sıralayıp ilk slotu döner
//        }

//        private AvailableDateTimeSlot FindCommonAvailableSlot(List<AvailableDateTimeSlot> slots1, List<AvailableDateTimeSlot> slots2) // Ortak kullanılabilir slotu bulan metot
//        {
//            foreach (var slot1 in slots1)
//            {
//                foreach (var slot2 in slots2)
//                {
//                    // İki slotun tarihi aynı mı kontrol edilir
//                    if (slot1.AvailableDate.Date == slot2.AvailableDate.Date)
//                    {
//                        // İki slotun saati aynı mı kontrol edilir
//                        if (slot1.AvailableHour == slot2.AvailableHour)
//                        {
//                            return slot1; // Slot1 veya slot2 döner, ikisi de aynıdır
//                        }
//                    }
//                }
//            }
//            return null; // Ortak slot bulunamazsa null döner
//        }
//    }
//}
