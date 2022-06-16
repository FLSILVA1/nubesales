using System.ComponentModel.DataAnnotations;


namespace NubeSalesMVC.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Senha Atual")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "Nova senha")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; }
    }
}
