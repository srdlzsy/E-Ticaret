using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class VerificationCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; } // Kodun geçerlilik süresi
    }

}
