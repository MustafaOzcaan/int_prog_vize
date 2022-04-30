using myoBlog29API.Models;
using myoBlog29API.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace int_prog_vize_projem.Controllers
{

    public class ServisController : ApiController
    {
        MyoBlog29DBEntities db = new MyoBlog29DBEntities();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]

        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KatMakaleSay = x.Soru.Count
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{katId}")]
        public KategoriModel KategoriById(int katId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.KategoriId == katId).Select(x => new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KatMakaleSay = x.Soru.Count
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.KategoriAdi == model.KategoriAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.KategoriAdi = model.KategoriAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.KategoriId == model.KategoriId).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.KategoriAdi = model.KategoriAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{katId}")]
        public SonucModel KategoriSil(int katId)
        {
            Kategori kayit = db.Kategori.Where(s => s.KategoriId == katId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Makale.Count(s => s.KategoriId == katId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Soru Kayıtlı Kategori Silinemez!";
                return sonuc;
            }

            db.Kategori.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion

        #region Soru


        [HttpGet]
        [Route("api/soruliste")]
        public List<SoruModel> MakaleListe()
        {
            List<SoruModel> liste = db.Makale.Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                Baslik = x.Baslik,
                Icerik = x.Icerik,
                Foto = x.Foto,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                Okunma = x.Okunma,
                UyeId = x.UyeId,
                UyeKadi = x.Uye.KullaniciAdi

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistesoneklenenler/{s}")]
        public List<SoruModel> MakaleListeSonEklenenler(int s)
        {
            List<SoruModel> liste = db.Makale.OrderByDescending(o => o.SoruId).Take(s).Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                Baslik = x.Baslik,
                Icerik = x.Icerik,
                Foto = x.Foto,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                Okunma = x.Okunma,
                UyeId = x.UyeId,
                UyeKadi = x.Uye.KullaniciAdi

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistebykatid/{katId}")]
        public List<SoruModel> MakaleListeByKatId(int katId)
        {
            List<SoruModel> liste = db.Makale.Where(s => s.KategoriId == katId).Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                Baslik = x.Baslik,
                Icerik = x.Icerik,
                Foto = x.Foto,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                Okunma = x.Okunma,
                UyeId = x.UyeId,
                UyeKadi = x.Uye.KullaniciAdi

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistebyuyeid/{uyeId}")]
        public List<SoruModel> MakaleListeByUyeId(int uyeId)
        {
            List<SoruModel> liste = db.Makale.Where(s => s.UyeId == uyeId).Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                Baslik = x.Baslik,
                Icerik = x.Icerik,
                Foto = x.Foto,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                Okunma = x.Okunma,
                UyeId = x.UyeId,
                UyeKadi = x.Uye.KullaniciAdi

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/sorubyid/{soruId}")]
        public SoruModel MakaleById(int makaleId)
        {
            SoruModel kayit = db.Makale.Where(s => s.SoruId == makaleId).Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                Baslik = x.Baslik,
                Icerik = x.Icerik,
                Foto = x.Foto,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                Okunma = x.Okunma,
                UyeId = x.UyeId,
                UyeKadi = x.Uye.KullaniciAdi
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/soruekle")]
        public SonucModel MakaleEkle(SoruModel model)
        {
            if (db.Makale.Count(s => s.Baslik == model.Baslik) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Soru Başlığı Kayıtlıdır!";
                return sonuc;
            }

            Makale yeni = new Makale();
            yeni.Baslik = model.Baslik;
            yeni.Icerik = model.Icerik;
            yeni.Okunma = model.Okunma;
            yeni.KategoriId = model.KategoriId;
            yeni.UyeId = model.UyeId;
            yeni.Foto = model.Foto;
            db.Makale.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/soruduzenle")]
        public SonucModel MakaleDuzenle(SoruModel model)
        {
            Makale kayit = db.Makale.Where(s => s.SoruId == model.SoruId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.Baslik = model.Baslik;
            kayit.Icerik = model.Icerik;
            kayit.Okunma = model.Okunma;
            kayit.KategoriId = model.KategoriId;
            kayit.UyeId = model.UyeId;
            kayit.Foto = model.Foto;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Düzenlendi";
            return sonuc;

        }

        [HttpDelete]
        [Route("api/sorusil/{soruId}")]
        public SonucModel MakaleSil(int makaleId)
        {
            Makale kayit = db.Makale.Where(s => s.SoruId == makaleId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Yorum.Count(s => s.SoruId == makaleId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Yorum Kayıtlı Soru Silinemez!";
                return sonuc;
            }

            db.Makale.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Silindi";
            return sonuc;
        }
        #endregion

        #region Üye

        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyeModel> UyeListe()
        {
            List<UyeModel> liste = db.Uye.Select(x => new UyeModel()
            {
                UyeId = x.UyeId,
                AdSoyad = x.AdSoyad,
                Email = x.Email,
                KullaniciAdi = x.KullaniciAdi,
                Foto = x.Foto,
                Sifre = x.Sifre,
                UyeAdmin = x.UyeAdmin
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public UyeModel UyeById(int uyeId)
        {
            UyeModel kayit = db.Uye.Where(s => s.UyeId == uyeId).Select(x => new UyeModel()
            {
                UyeId = x.UyeId,
                AdSoyad = x.AdSoyad,
                Email = x.Email,
                KullaniciAdi = x.KullaniciAdi,
                Foto = x.Foto,
                Sifre = x.Sifre,
                UyeAdmin = x.UyeAdmin
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyeModel model)
        {
            if (db.Uye.Count(s => s.KullaniciAdi == model.KullaniciAdi || s.Email == model.Email) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kullanıcı Adı veya E-Posta Adresi Kayıtlıdır!";
                return sonuc;
            }

            Uye yeni = new Uye();
            yeni.AdSoyad = model.AdSoyad;
            yeni.Email = model.Email;
            yeni.KullaniciAdi = model.KullaniciAdi;
            yeni.Foto = model.Foto;
            yeni.Sifre = model.Sifre;
            yeni.UyeAdmin = model.UyeAdmin;

            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyeModel model)
        {
            Uye kayit = db.Uye.Where(s => s.UyeId == model.UyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            kayit.AdSoyad = model.AdSoyad;
            kayit.Email = model.Email;
            kayit.KullaniciAdi = model.KullaniciAdi;
            kayit.Foto = model.Foto;
            kayit.Sifre = model.Sifre;
            kayit.UyeAdmin = model.UyeAdmin;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public SonucModel UyeSil(int uyeId)
        {
            Uye kayit = db.Uye.Where(s => s.UyeId == uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            if (db.Makale.Count(s => s.UyeId == uyeId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Soru Kaydı Olan Üye Silinemez!";
                return sonuc;
            }

            if (db.Yorum.Count(s => s.UyeId == uyeId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Yorum Kaydı Olan Üye Silinemez!";
                return sonuc;
            }

            db.Uye.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }
        #endregion

        #region Yorum

        [HttpGet]
        [Route("api/yorumliste")]
        public List<YorumModel> YorumListe()
        {
            List<YorumModel> liste = db.Yorum.Select(x => new YorumModel()
            {
                YorumId = x.YorumId,
                YorumIcerik = x.YorumIcerik,
                MakaleId = x.SoruId,
                UyeId = x.UyeId,
                Tarih = x.Tarih,
                KullaniciAdi = x.Uye.KullaniciAdi,
                MakaleBaslik = x.Soru.Baslik,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/yorumlistebyuyeid/{uyeId}")]
        public List<YorumModel> YorumListeByUyeId(int uyeId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.UyeId == uyeId).Select(x => new YorumModel()
            {
                YorumId = x.YorumId,
                YorumIcerik = x.YorumIcerik,
                MakaleId = x.SoruId,
                UyeId = x.UyeId,
                Tarih = x.Tarih,
                KullaniciAdi = x.Uye.KullaniciAdi,
                MakaleBaslik = x.Soru.Baslik,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/yorumlistebySoruid/{soruId}")]
        public List<YorumModel> YorumListeBymakaleId(int makaleId)
        {
            List<YorumModel> liste = db.Yorum.Where(s => s.SoruId == makaleId).Select(x => new YorumModel()
            {
                YorumId = x.YorumId,
                YorumIcerik = x.YorumIcerik,
                MakaleId = x.SoruId,
                UyeId = x.UyeId,
                Tarih = x.Tarih,
                KullaniciAdi = x.Uye.KullaniciAdi,
                MakaleBaslik = x.Soru.Baslik,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/yorumlistesoneklenenler/{s}")]
        public List<YorumModel> YorumListeSonEklenenler(int s)
        {
            List<YorumModel> liste = db.Yorum.OrderByDescending(o => o.SoruId).Take(s).Select(x => new YorumModel()
            {
                YorumId = x.YorumId,
                YorumIcerik = x.YorumIcerik,
                MakaleId = x.SoruId,
                UyeId = x.UyeId,
                Tarih = x.Tarih,
                KullaniciAdi = x.Uye.KullaniciAdi,
                MakaleBaslik = x.Soru.Baslik,

            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/yorumbyid/{yorumId}")]

        public YorumModel YorumById(int yorumId)
        {
            YorumModel kayit = db.Yorum.Where(s => s.YorumId == yorumId).Select(x => new YorumModel()
            {
                YorumId = x.YorumId,
                YorumIcerik = x.YorumIcerik,
                MakaleId = x.SoruId,
                UyeId = x.UyeId,
                Tarih = x.Tarih,
                KullaniciAdi = x.Uye.KullaniciAdi,
                MakaleBaslik = x.Soru.Baslik,
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/yorumekle")]
        public SonucModel YorumEkle(YorumModel model)
        {
            if (db.Yorum.Count(s => s.UyeId == model.UyeId && s.SoruId == model.MakaleId && s.YorumIcerik == model.YorumIcerik) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Aynı Kişi, Aynı Soruya Aynı Yorumu Yapamaz!";
                return sonuc;
            }

            Yorum yeni = new Yorum();
            yeni.YorumId = model.YorumId;
            yeni.YorumIcerik = model.YorumIcerik;
            yeni.SoruId = model.MakaleId;
            yeni.UyeId = model.UyeId;
            yeni.Tarih = model.Tarih;
            db.Yorum.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Eklendi";

            return sonuc;
        }
        [HttpPut]
        [Route("api/yorumduzenle")]
        public SonucModel YorumDuzenle(YorumModel model)
        {

            Yorum kayit = db.Yorum.Where(s => s.YorumId == model.YorumId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.YorumId = model.YorumId;
            kayit.YorumIcerik = model.YorumIcerik;
            kayit.SoruId = model.MakaleId;
            kayit.UyeId = model.UyeId;
            kayit.Tarih = model.Tarih;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/yorumsil/{yorumId}")]
        public SonucModel YorumSil(int yorumId)
        {
            Yorum kayit = db.Yorum.Where(s => s.YorumId == yorumId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            db.Yorum.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Yorum Silindi";

            return sonuc;
        }


        #endregion
    }

    public class SoruModel
    {
        public int SoruId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string Foto { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int UyeId { get; set; }
        public string UyeKadi { get; set; }
        public int Okunma { get; set; }
    }
}
