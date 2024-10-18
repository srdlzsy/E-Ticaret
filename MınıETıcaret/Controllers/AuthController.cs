using Application.Dtos;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JWT _jwtService;
    private readonly UserRepository _userRepository;
    private static readonly Dictionary<string, string> _confirmationCodes = new();

    public AuthController(JWT jwtService, UserRepository userRepository)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        // Aynı email'e sahip bir kullanıcı var mı kontrol et
        if (_userRepository.IsEmailRegistered(registerDto.Email))
        {
            return BadRequest("Bu email ile kayıtlı bir kullanıcı zaten mevcut.");
        }

        // Şifreyi hashle
        var hashedPassword = HashPassword(registerDto.PasswordHash);

        // Kullanıcıyı oluştur
        var user = new User
        {
            UserName = registerDto.Email, // Kullanıcı adı olarak e-posta kullanılıyor
            Email = registerDto.Email,
            PasswordHash = hashedPassword, // Hashlenmiş şifreyi kullan
            IsEmailVerified = false // Doğrulama bekleniyor
        };

        // Kullanıcıyı geçici olarak ekle, e-posta doğrulaması sonrasında aktif edilecek
        _userRepository.AddUser(user);

        // Benzersiz bir doğrulama kodu üret (Örn: 6 haneli random bir sayı)
        var confirmationCode = GenerateConfirmationCode();

        // E-posta gönderme işlemi
        SendEmailConfirmationCode(registerDto.Email, confirmationCode);

        // Doğrulama kodunu geçici olarak sakla
        _confirmationCodes[registerDto.Email] = confirmationCode;

        return Ok("Kullanıcı kayıt işlemi başlatıldı. E-posta adresinize doğrulama kodu gönderildi.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto loginDto)
    {
        // Kullanıcıyı e-posta ile bul
        var user = _userRepository.GetUserByEmail(loginDto.Email);

        if (user == null)
        {
            return BadRequest("Kullanıcı bulunamadı.");
        }

        // Şifreyi doğrula
        if (user.PasswordHash != loginDto.PasswordHash)
        {
            return Unauthorized("Hatalı şifre.");
        }

        // Eğer kullanıcı e-posta doğrulamasını tamamlamamışsa
        if (!user.IsEmailVerified)
        {
            return Unauthorized("E-posta adresi doğrulanmamış.");
        }

        // JWT token oluştur
        var token = _jwtService.GenerateJwtToken(user.UserId.ToString());

        return Ok(new { Token = token });
    }

    // E-posta doğrulama kodunu gönder
    [HttpPost("send-confirmation-code")]
    public IActionResult SendConfirmationCode([FromBody] EmailDto emailDto)
    {
        if (string.IsNullOrEmpty(emailDto.Email))
        {
            return BadRequest("E-posta adresi gereklidir.");
        }

        // Basit bir doğrulama kodu üret
        var confirmationCode = GenerateConfirmationCode();

        // E-posta gönderim işlemi
        SendEmailConfirmationCode(emailDto.Email, confirmationCode);

        // Doğrulama kodunu geçici olarak sakla
        _confirmationCodes[emailDto.Email] = confirmationCode;

        return Ok("Doğrulama kodu e-postanıza gönderildi.");
    }

    // Doğrulama kodunu kontrol et ve kaydı tamamla
    [HttpPost("verify-confirmation-code")]
    public IActionResult VerifyConfirmationCode([FromBody] VerificationCodeDto verifyCodeDto)
    {
        if (_confirmationCodes.TryGetValue(verifyCodeDto.Email, out var storedCode) && storedCode == verifyCodeDto.Code)
        {
            // Doğrulama başarılı, kullanıcıyı ekle
            var user = new User
            {
                UserName = verifyCodeDto.Email, // Kullanıcı adı olarak e-posta kullanılıyor
                Email = verifyCodeDto.Email,
                IsEmailVerified = true // E-postayı doğrula
            };

            _userRepository.AddUser(user); // Kullanıcıyı ekle
            _confirmationCodes.Remove(verifyCodeDto.Email); // Doğrulama kodunu temizle
            return Ok("Kullanıcı başarıyla kaydedildi.");
        }

        return BadRequest("Geçersiz doğrulama kodu.");
    }

    // E-posta doğrulama kodunu gönderen fonksiyon
    private void SendEmailConfirmationCode(string email, string confirmationCode)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your Name", "srdlzsyy@gmail.com")); // Kendi adınızı ve e-posta adresinizi ekleyin
        message.To.Add(new MailboxAddress("", email)); // Dinamik olarak kullanıcının e-posta adresi
        message.Subject = "Doğrulama Kodu"; // E-posta konusu
        message.Body = new TextPart("html")
        {
            Text = $"Doğrulama kodunuz: <strong>{confirmationCode}</strong>" // E-posta içeriği
        };

        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls); // Gmail SMTP ayarları
                client.Authenticate("srdlzsyy@gmail.com", "agredfbqeqmjjcyo"); // Gmail e-posta adresiniz ve uygulama şifresi
                client.Send(message);
                Console.WriteLine($"E-posta gönderildi: {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderim hatası: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
            }
        }
    }

    // Benzersiz doğrulama kodu üreten fonksiyon
    private string GenerateConfirmationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 haneli random sayı
    }
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }


}
