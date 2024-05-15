using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using Google.Cloud.Firestore.V1;
using System.Linq;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {
        private async Task<bool> IsStudentAuthorized(string username)
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // /evaluation-project/UserLogin/Student yolunda kayıtlı öğrenci numaralarını al
            CollectionReference studentCollection = db.Collection("evaluation-project/UserLogin/Student");
            QuerySnapshot snapshot = await studentCollection.GetSnapshotAsync();

            // Firestore'dan alınan öğrenci numaralarını bir listeye dönüştür
            List<string> authorizedStudentIds = snapshot.Documents
                .Select(doc => doc.GetValue<string>("StudentID"))
                .ToList();

            // Giriş yapan öğrencinin numarasının yetkili listede olup olmadığını kontrol et
            return authorizedStudentIds.Contains(username);
        }

        private async Task AddUserLoginToFirestore(string username)
        {
            // Firestore projenizin kimlik dosyasının yolunu belirtin
            string path = AppDomain.CurrentDomain.BaseDirectory + @"evaluation-project-eed94-firebase-adminsdk-boj92-f073cfbb8f.json";

            // FirestoreDb nesnesini oluşturmak için Firestore projesinin kimliğini kullanın
            FirestoreDb db = FirestoreDb.Create(projectId: "evaluation-project-eed94", new FirestoreClientBuilder
            {
                CredentialsPath = path
            }.Build());

            // Kullanıcının giriş tarihini alın
            DateTime loginTime = DateTime.UtcNow;
            CollectionReference collection = db.Collection("evaluation-project");

            // Kullanıcı verisini hazırlar
            var data = new Dictionary<string, object>();

            data.Add("LoginTime", loginTime);

            if (username.All(char.IsDigit))
            {
                DocumentReference document = collection.Document("UserLogin");
                CollectionReference subcollection = document.Collection("Student");
                DocumentReference refer = subcollection.Document(username); // Kullanıcı adını belge kimliği olarak kullanır

                data.Add("StudentID", username);

                await refer.SetAsync(data); // Belgeyi SetAsync kullanarak ekler
            }
            else
            {
                DocumentReference document = collection.Document("Professor");
                CollectionReference subcollection = document.Collection("Academian");
                DocumentReference refer = subcollection.Document(username); // Firestore'a belge eklerken otomatik bir kimlik atanmasını sağlar

                data.Add("ProfessorID", username);

                await refer.SetAsync(data); // Belgeyi SetAsync kullanarak ekler
            }

            Console.WriteLine("User data has been successfully added to Firestore.");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                ChromeOptions optionsc = new ChromeOptions();
                optionsc.AddArgument("--headless");
                optionsc.AddArguments("--disable-gpu", "--no-sandbox", "--start-maximized --headless");

                // Chrome tarayıcısını başlat
                using (var driver = new ChromeDriver(options: optionsc))
                {
                    // Web sitesine git
                    driver.Navigate().GoToUrl("https://cats.iku.edu.tr/access/login");

                    // Kullanıcı adı ve şifreyi doldur
                    var usernameInput = driver.FindElement(By.Id("eid")); // Kullanıcı adı alanını bul
                    var passwordInput = driver.FindElement(By.Id("pw")); // Şifre alanını bul

                    usernameInput.SendKeys(username); // Kullanıcı adını giriş kutusuna gir
                    passwordInput.SendKeys(password); // Şifreyi giriş kutusuna gir

                    // Giriş butonunu bul ve tıkla
                    var loginButton = driver.FindElement(By.Id("submit")); // Giriş butonunu bul
                    loginButton.Click(); // Giriş butonuna tıkla

                    // "Mrphs-userNav__submenuitem--profilepicture" sınıfının varlığını kontrol eder
                    if (driver.FindElement(By.ClassName("Mrphs-userNav__submenuitem--profilepicture")) != null)
                    {
                        // Kullanıcıyı Firestore'a ekle
                        await AddUserLoginToFirestore(username);

                        TempData["Username"] = username; // Kullanıcı adını TempData üzerinden aktar

                        if (username.All(char.IsDigit))
                        {
                            // Öğrenci numarasını Firestore'dan çekip yetkilendirme işlemini gerçekleştir
                            if (await IsStudentAuthorized(username))
                            {
                                // Yetkili öğrenci, öğrenci sayfasına yönlendir
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                // Yetkisiz öğrenci, hata mesajı göster
                                TempData["ErrorMessage"] = "You are not authorized to access this page.";
                                return RedirectToAction("Login", "Login");
                            }
                        }
                        else
                        {
                            // Profesör, profesör sayfasına yönlendir
                            return RedirectToAction("Admin", "Home");
                        }
                    }
                    else
                    {
                        // Kullanıcı bilgileri yanlış
                        TempData["ErrorMessage"] = "The username or password is incorrect. Please try again.";
                        return RedirectToAction("Login", "Login"); // Giriş sayfasına yeniden yönlendirme
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemler
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
                return RedirectToAction("Login", "Login"); // Giriş sayfasına yeniden yönlendirme
            }
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}