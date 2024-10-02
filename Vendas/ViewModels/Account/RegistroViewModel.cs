using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels.Account
{
    public class RegistroViewModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "Formato do email inválido")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Required]
        [Display(Name = "Confrime a Senha")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        [DataType(DataType.Password)]
        public string? ConfirmarSenha { get; set; }
    }
}
