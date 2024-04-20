using System.ComponentModel.DataAnnotations;

namespace ApiHelpet.ViewModels
{
    public class AnimalViewModel
    {
        [Required(ErrorMessage = "A descrição do animal é obrigatória.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O tamanho do animal é obrigatório.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "A cor do animal é obrigatória.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "A idade do animal é obrigatória.")]
        public string Age { get; set; }

        [Required(ErrorMessage = "A raça do animal é obrigatória.")]
        public string Race { get; set; }

        [Range(0, 5, ErrorMessage = "O campo tipo deve ser de 0 a 5.")]
        public int Type { get; set; }
    }
}
