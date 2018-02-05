using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobitBilismDgerlendirmeProjesi.Models;
using System.IO;
using System.Net.Mail;
using System.Net;



namespace MobitBilismDgerlendirmeProjesi.Controllers
{
    public class HomeController : Controller
    {// Veritabnı Context Çağırmak
        MobitDatabaseContext Veritabani = new MobitDatabaseContext();

        #region KullaniciyaListeleme (AnaSayfa)
        public ActionResult Index()
        {
            ViewBag.MailDurum = "";
            var UrunListesi = Veritabani.Urunler.ToList().OrderByDescending
            (x => x.Id);
            return View(UrunListesi);

        }
        [Route("{UrunAdi}")]
        public ActionResult IndexDetay(int Id)
        {

            ViewBag.MailDurum = "";
            var detay = Veritabani.Urunler.Find(Id);

            //RouteAttribute route = detay.UrunAdi;
            return View(detay);

        }
        #endregion

        #region Kullanıcı Girş Ve Session
        //Bu kısmda kullanıcı girişi ve session oluşturma yani login olayları var.
        // Sayfa çağrıldığında Email ve şifre girişi istiyor , giriş butonuna basıldığında eğer kullanıcı kayıtlıysa Session Açıp Admin Paneline Yönlendiriyor...
        //Eğer kullanıcı mevcut değilse veya bilgileri yanlışsa kendini çağırıyor ve hata mesajı dönürüyor.
        // Admin paneli ve bağlantılı alt Viewler açılmadan önce session kontrolü yapıyorlar 
        //Session Yoksa Açılmıyor Giris sayfasına yönlendiriyorlar. Böylelikle giriş yapılmadan veri CRUD işlemleri yapılmasına izin vermiyoruz
        // Ayrıca admin paneline girmiş kullanıcların ek olarak yetkileri kontrol edilerik yetkisiz işlem yapmasına izin vermiyorum...




        [HttpGet]
        public ActionResult Giris()
        {


            return View();

        }

        [HttpPost]
        public ActionResult Giris(Kullanicilar GelenVeriler)
        {

            try
            {
                var Kullanici = Veritabani.Kullanicilar.Where(x => x.Email == GelenVeriler.Email && x.Sifre == GelenVeriler.Sifre).FirstOrDefault();




                if (Kullanici != null)
                {
                    Session["KullaniciAdiSoyadi"] = Kullanici.AdiSoyadi;
                    Session["Email"] = GelenVeriler.Email;
                    Session["Yetkisi"] = Kullanici.Yetkisi;
                    ViewBag.GirisYap = "";

                    return RedirectToAction("AdminPaneli");
                }

                else { ViewBag.GirisYap = "Kullanici Email vey Şifreniz hatalıdır..."; return View(); }


            }
            catch (Exception Hata)
            {

                ViewBag.GirisYap = "Server ile bağlantı kurulamadı!" + Hata.ToString();
                return View();
            }


        }

        public ActionResult Cikis()
        {    // Action Link ile çıkış actionresult'u çağrılınca session u siliyor ve kullanıcıyı indexe yolluyor.  

            Session.RemoveAll();
            return RedirectToAction("Index");
        }


        #endregion

        #region AdminPaneliListleme

        // Admin Paneli Listeleme Sayfası
        public ActionResult AdminPaneli()
        {
            // Sesion olup olmadığının kontrolü yoksa Giriş sayfasına yönlendiriliyor.
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                var UrunListesi = Veritabani.Urunler.ToList().OrderByDescending
            (x => x.Id);

                return View(UrunListesi);
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }
        }
        #endregion
        
        #region AdminYeniUrunGirme

        [HttpGet]
        public ActionResult Ekle()
        {
            // Sesion olup olmadığının kontrolü yoksa Giriş sayfasına yönlendiriliyor.
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                return View();
            }
            else // Session Yoksa Yönlendirme Yapılıyor Giriş izni yok
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }

        }

        [HttpPost]
        public ActionResult Ekle(Urunler GirilenDegerler, HttpPostedFileBase uploadfile)
        {
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {// Kullanıcının girmiş olduğu değerleri veritabanıma aktarıyorum eğer sesion varsa...

                try
                {
                    if (uploadfile.ContentLength > 0)
                    {
                        ViewBag.ResimHatsi = "";
                        string resimguid = Guid.NewGuid().ToString();
                        string filePath = Path.Combine(Server.MapPath("~/Content/images"), resimguid + Path.GetFileName(uploadfile.FileName));
                        uploadfile.SaveAs(filePath);
                        GirilenDegerler.UrunResimYolu = resimguid + Path.GetFileName(uploadfile.FileName);

                    }
                }
                catch (Exception)
                {
                    ViewBag.ResimHatsi = "Hata: Resim seçiniz..."; return View();

                }


                Veritabani.Urunler.Add(GirilenDegerler);
                Veritabani.SaveChanges();
                return RedirectToAction("AdminPaneli");

            }
            else // Session Yoksa Yönlendirme Yapılıyor Giriş izni yok
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }



        }
        #endregion

        #region AdminUrunDuzenleme
        [HttpGet]
        public ActionResult Duzenle(int id)
        {// Sesion olup olmadığının kontrolü yoksa Giriş sayfasına yönlendiriliyor.
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                var SecilenUrunBilgileri = Veritabani.Urunler.Find(id);
                return View(SecilenUrunBilgileri);
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }

        }



        [HttpPost]
        public ActionResult Duzenle(Urunler DuzenlenmisVeriler, HttpPostedFileBase uploadfile)
        {
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {


                Urunler EskiUrun = Veritabani.Urunler.Find(DuzenlenmisVeriler.Id);
                EskiUrun.UrunAdi = DuzenlenmisVeriler.UrunAdi;
                //  EskiUrun.UrunResimYolu = DuzenlenmisVeriler.UrunResimYolu;
                EskiUrun.UrunAciklamasi = DuzenlenmisVeriler.UrunAciklamasi;
                try
                {

                    if (uploadfile.ContentLength > 0)
                    {

                        string resimguid = Guid.NewGuid().ToString();
                        string filePath = Path.Combine(Server.MapPath("~/Content/images"), resimguid + Path.GetFileName(uploadfile.FileName));
                        uploadfile.SaveAs(filePath);
                        EskiUrun.UrunResimYolu = resimguid + Path.GetFileName(uploadfile.FileName);
                    }

                }
                catch (Exception)
                {
                    EskiUrun.UrunResimYolu = EskiUrun.UrunResimYolu;

                }

                Veritabani.SaveChanges();
                return RedirectToAction("AdminPaneli");
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }

        }
        #endregion

        #region AdminDetayGörüntüleme
        [HttpGet]
        public ActionResult Detay(int id)
        {
            // Sesion olup olmadığının kontrolü yoksa Giriş sayfasına yönlendiriliyor.
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                var SecilenUrunBilgileri = Veritabani.Urunler.Find(id);
                return View(SecilenUrunBilgileri);
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }

        }
        #endregion

        #region AdminSil
        [HttpGet]
        public ActionResult Sil(int id)
        {
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                var SilinecekUrunBilgileri = Veritabani.Urunler.Find(id);
                return View(SilinecekUrunBilgileri);
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }

        }
        public ActionResult Sil(Urunler Silinecekurun)
        {
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                Urunler Slineceek = Veritabani.Urunler.Find(Silinecekurun.Id);
                Veritabani.Urunler.Remove(Slineceek);
                Veritabani.SaveChanges();
                return RedirectToAction("AdminPaneli");
            }
            else
            {
                ViewBag.GirisYap = "Lütfen giriş Yapınız";
                return RedirectToAction("Giris");
            }


        }
        #endregion

        #region MailGonderme Partial
        [HttpGet]
        public ActionResult MailPartial()
        {


            return PartialView();
        }

        [HttpPost]
        public ActionResult MailPartial(EmailModel EmailAlanlari)
        {
            try
            {

                SmtpClient server = new SmtpClient("smtp.muratcoskuncv.com");
            server.Credentials = new NetworkCredential("info@muratcoskuncv.com", "Mail123?");
            MailMessage email = new MailMessage();
            email.From = new MailAddress("info@muratcoskuncv.com");

            email.To.Add(EmailAlanlari.Kime);
            email.Subject = EmailAlanlari.Baslik;
            email.Body = EmailAlanlari.Icerik + " Sayfa Url: " + Request.Url.ToString();

           
                server.Send(email);
            }
            catch (SmtpFailedRecipientException hata)
            {
                ViewBag.MailDurum = "Hata işlem başarısız... Detay: " + hata;
                return View();
            }

            ViewBag.MailDurum = "Email gönderme işlemi başarılı...";
            ModelState.Clear();

            return View();
        }


        #endregion


        public ActionResult AciklamaVeProjeDosylari()
        {
            if (Session["Email"] != null && Session["Yetkisi"] != null)
            {
                return View();
            }
            else { return RedirectToAction("Giris"); }
        }
    }
}