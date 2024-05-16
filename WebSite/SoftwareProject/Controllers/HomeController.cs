using Google.Cloud.Firestore.V1;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SoftwareProject.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SoftwareProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult File()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }

        public IActionResult NecessaryDocument()
        {
            return View();
        }

        private async Task<bool> IsStudentAllowed(string username)
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            try
            {
                // Koleksiyon referansını alın
                CollectionReference collection = db.Collection("evaluation-project/UserLogin/Student");

                // Tüm dokümanları çekin
                QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

                // Dokümanlar arasında döngü kurarak öğrenci numarasını arayın
                foreach (DocumentSnapshot documentSnapshot in querySnapshot)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> studentData = documentSnapshot.ToDictionary();

                        // Öğrenci numarası bulunduysa true döndür
                        if (studentData.ContainsKey("StudentID") && studentData["StudentID"].ToString() == username)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking student authorization: " + ex.Message);
                // Hata durumunda yetkilendirme başarısız sayılır
                return false;
            }

            // Öğrenci numarası bulunamadıysa false döndür
            return false;
        }

        public async Task<IActionResult> Admin()
        {
            // Kullanıcının giriş yapmış olması kontrol ediliyor
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string username = TempData["Username"]?.ToString();

            try
            {
                if (int.TryParse(username, out _))
                {
                    // Öğrenci girişi
                    if (await IsStudentAllowed(username))
                    {
                        // Yetkili öğrenci, hata mesajı göster 
                        // Öğrenciler Admin sayfasına erişememeli
                        TempData["ErrorMessage"] = "You are not authorized to view this page.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Yetkisiz öğrenci, hata mesajı göster
                        TempData["ErrorMessage"] = "You are not authorized to view this page.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Profesör girişi, Admin sayfasına yönlendir
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemler
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // *    Create Metotları  * ///

        public async Task AddDataAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Yeni bir koleksiyon ve belge referansı oluşturun
            CollectionReference collection = db.Collection("evaluation-project");
            DocumentReference document = collection.Document("hahaha");

            // Belgeyi Firestore veritabanına eklemek için bir koleksiyon oluşturun
            CollectionReference subcollection = document.Collection("test");

            // Eklemek istediğiniz veri (örneğin bir obje) hazırlayın
            var data = new
            {
                Name = "Onur",
                Age = 22,
                City = "İstanbul"
            };

            // Belgeyi Firestore veritabanına ekleyin
            await subcollection.AddAsync(data);

            Console.WriteLine("Veri başarıyla eklendi.");
        }

        // *    Read Metotları  * ///

        public async Task<List<Dictionary<string, object>>> GetProjectDataAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Belirli bir koleksiyon referansı oluşturun
            CollectionReference collection = db.Collection("evaluation-project/Project/GraduationProject");

            // Koleksiyondaki tüm belgeleri almak için bir sorgu oluşturun
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

            // Belgeleri depolamak için bir liste oluşturun
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

            // Her belge için verileri alın
            foreach (DocumentSnapshot documentSnapshot in querySnapshot)
            {
                if (documentSnapshot.Exists)
                {
                    // Belgedeki verileri alın
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();

                    // Verileri listeye ekle
                    documents.Add(data);
                }
            }

            return documents;
        }

        public async Task<List<Dictionary<string, object>>> GetProfessorDataAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Belirli bir koleksiyon referansı oluşturun
            CollectionReference collection = db.Collection("evaluation-project/Professor/Academician");

            // Koleksiyondaki tüm belgeleri almak için bir sorgu oluşturun
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

            // Belgeleri depolamak için bir liste oluşturun
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

            // Her belge için verileri alın
            foreach (DocumentSnapshot documentSnapshot in querySnapshot)
            {
                if (documentSnapshot.Exists)
                {
                    // Belgedeki verileri alın
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();

                    // Verileri listeye ekle
                    documents.Add(data);
                }
            }

            return documents;
        }

        public async Task<List<Dictionary<string, object>>> GetBookedDatesDataAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Belirli bir koleksiyon referansı oluşturun
            CollectionReference collection = db.Collection("evaluation-project/BookedDates/DatesThatBooked");

            // Koleksiyondaki tüm belgeleri almak için bir sorgu oluşturun
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

            // Belgeleri depolamak için bir liste oluşturun
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

            // Her belge için verileri alın
            foreach (DocumentSnapshot documentSnapshot in querySnapshot)
            {
                if (documentSnapshot.Exists)
                {
                    // Belgedeki verileri alın
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();

                    // Verileri listeye ekle
                    documents.Add(data);
                }
            }

            return documents;
        }

        public async Task<List<Dictionary<string, object>>> GetAvailableDateTimeDataAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Belirli bir koleksiyon referansı oluşturun
            CollectionReference collection = db.Collection("evaluation-project/AvailableDateTimeSlot/AvailableDates");

            // Koleksiyondaki tüm belgeleri almak için bir sorgu oluşturun
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

            // Belgeleri depolamak için bir liste oluşturun
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

            // Her belge için verileri alın
            foreach (DocumentSnapshot documentSnapshot in querySnapshot)
            {
                if (documentSnapshot.Exists)
                {
                    // Belgedeki verileri alın
                    Dictionary<string, object> data = documentSnapshot.ToDictionary();

                    // Verileri listeye ekle
                    documents.Add(data);
                }
            }

            return documents;
        }

        public async Task<IActionResult> Index()
        {
            AddDataAsync().Wait();

            //project
            List<Dictionary<string, object>> docs1 = await GetProjectDataAsync();

            foreach (var document in docs1)
            {
                Console.WriteLine("Projects:");

                foreach (var field in document)
                {
                    Console.WriteLine($"{field.Key}: {field.Value}");
                }

                Console.WriteLine();
            }

            //prof
            List<Dictionary<string, object>> docs2 = await GetProfessorDataAsync();

            foreach (var document in docs2)
            {
                Console.WriteLine("Professors:");

                foreach (var field in document)
                {
                    Console.WriteLine($"{field.Key}: {field.Value}");
                }
                Console.WriteLine();
            }

            //available dates
            List<Dictionary<string, object>> docs3 = await GetAvailableDateTimeDataAsync();

            foreach (var document in docs3)
            {
                Console.WriteLine("AvailableDates:");

                foreach (var field in document)
                {
                    Console.WriteLine($"{field.Key}: {field.Value}");
                }

                Console.WriteLine();
            }

            //bookeddates
            List<Dictionary<string, object>> docs4 = await GetBookedDatesDataAsync();

            foreach (var document in docs4)
            {
                Console.WriteLine("BookedDates:");

                foreach (var field in document)
                {
                    Console.WriteLine($"{field.Key}: {field.Value}");
                }
                Console.WriteLine();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> takvim()
        {
            // Kullanıcının giriş yapmış olması kontrol ediliyor
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string username = TempData["Username"]?.ToString();

            try
            {
                if (int.TryParse(username, out _))
                {
                    // Öğrenci girişi
                    if (await IsStudentAllowed(username))
                    {
                        // Yetkili öğrenci, hata mesajı göster 
                        // Öğrenciler takvime erişememeli
                        TempData["ErrorMessage"] = "You are not authorized to view this page.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Yetkisiz öğrenci, hata mesajı göster
                        TempData["ErrorMessage"] = "You are not authorized to view this page.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Profesör girişi, takvim sayfasına yönlendir
                    var model = new CalendarViewModel
                    {
                        Events = new List<Event>
                {
                    new Event { Title = "Örnek Etkinlik", Date = DateTime.Now, Description = "Bu bir örnek etkinliktir." }
                    // İhtiyacınıza göre burada daha fazla etkinlik ekleyebilirsiniz
                }
                    };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemler
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task<bool> IsStudentAuthorized(string studentId)
        {
            // Firestore'dan öğrenci ID'lerini çek
            List<string> studentIds = await GetStudentIdsAsync();

            // Giriş yapan öğrencinin ID'si Firestore listesinde varsa true, yoksa false döndür
            return studentIds.Contains(studentId);
        }

        private async Task<List<string>> GetStudentIdsAsync()
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // "evaluation-project/UserLogin/Student" koleksiyonuna referans alın
            CollectionReference collection = db.Collection("evaluation-project/UserLogin/Student");

            // Koleksiyondaki tüm belgeleri çek
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();

            // Öğrenci ID'lerini tutacak bir liste
            List<string> studentIds = new List<string>();

            // Belgeler üzerinde dönerek öğrenci ID'lerini listeye ekle
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                studentIds.Add(documentSnapshot.Id);
            }

            return studentIds;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}