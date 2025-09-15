# WebApplication2 API

WebApplication2, ASP.NET Core ile geliştirilmiş, kullanıcı ve departman yönetimi sağlayan bir Web API projesidir. Rol tabanlı yetkilendirme (Admin & Employer), JWT tabanlı authentication ve e-posta ile şifre sıfırlama özelliklerini içerir.

---

## 🚀 Özellikler

- Kullanıcı kayıt, giriş, çıkış işlemleri (JWT Cookie kullanımı)
- Şifre sıfırlama ve güncelleme e-posta desteği
- Admin rolü ile personel ve departman yönetimi
- Employer rolü ile profil görüntüleme ve güncelleme
- Role bazlı yetkilendirme
- Swagger ile API dokümantasyonu
- Entity Framework Core ile SQL Server veri tabanı entegrasyonu
- SeedData ile başlangıç kullanıcı ve rollerinin oluşturulması

---

## 🛠 Kullanılan Teknolojiler

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- JWT Authentication
- Swagger / OpenAPI
- SQL Server
- SMTP E-posta Servisi (Gmail)

---

🔗 API Endpointleri
AccountController
Endpoint	Method	Açıklama
/api/account/register	POST	Yeni kullanıcı kaydı
/api/account/login	POST	Kullanıcı giriş
/api/account/logout	POST	Çıkış yapma
/api/account/forgot-password	POST	Şifre sıfırlama e-postası gönderme
/api/account/reset-password	POST	Şifreyi resetleme


AdminController (Admin Rolü)
Endpoint	Method	Açıklama
/api/admin/personel-list	GET	Tüm personeli listeleme
/api/admin/personel-create	POST	Yeni personel oluşturma
/api/admin/personel-update/{id}	PUT	Personel güncelleme
/api/admin/personel-delete/{id}	DELETE	Personel silme
/api/admin/department-list	GET	Tüm departmanları listeleme
/api/admin/create-department	POST	Yeni departman oluşturma
/api/admin/update-department/{id}	PUT	Departman güncelleme
/api/admin/delete-department/{id}	DELETE	Departman silme


EmployerController (Employer Rolü)
Endpoint	Method	Açıklama
/api/employer/profile	GET	Profil bilgilerini görüntüleme
/api/employer/update-profile	PUT	Profil güncelleme (şifre dahil)
