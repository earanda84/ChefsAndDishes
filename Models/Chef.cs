#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace ChefsAndDishes.Models
{
    public class Chef
    {
        [Key]
        public int ChefId { get; set; }
        // FirstName
        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        // LastName
        [Required]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        // DateOfBird
        [Required]
        [Display(Name = "Date of Birth:")]
        [PreviousDateOfBirth]
        public DateTime DateOfBirth { get; set; }

        // CreatedAt
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // UpdatedAt
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public List<Dish> AllDishes { get; set; } = new List<Dish>();

        // Calculo de Edad
        // Propiedad de solo lectura para mostrar la edad basada en la fecha de nacimiento.
        public int Age
        {
            // Método get para calcular la edad.
            get
            {
                // Calcula la diferencia de años entre el año actual y el año de nacimiento.
                int age = DateTime.Now.Year - DateOfBirth.Year;

                // Ajusta la edad si aún no ha pasado la fecha de cumpleaños de este año.
                if (DateTime.Now < DateOfBirth.AddYears(age))
                {
                    // Si aún no ha pasado la fecha de cumpleaños de este año, ajusta la edad disminuyéndola en 1.
                    age--;
                }

                // Retorna la edad calculada.
                return age;
            }
        }
    }

    // Validation of Birth
    public class PreviousDateOfBirthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Evalúa si el tipo de dato del objeto es DateTime, lo asigna a la variable date
            if (value is DateTime date)
            {
                // Evalúa si la fecha pasada en el formulario es >= a La fecha de hoy
                if (date >= DateTime.Now)
                {
                    // Retorna un resultado de la validación
                    return new ValidationResult("La fecha de nacimiento debe ser anterior a la fecha de hoy");
                }

                // Se crea variable mínima con 18
                int minAge = 18;

                // Evalúa si la fecha actual reducida en la cantidad mínima de años, es menor a la fecha proporcionada.
                if (DateTime.Now.AddYears(-minAge) < date)
                {
                    return new ValidationResult($"El Chef debe tener al menos {minAge} años de edad");
                }
            }

            return ValidationResult.Success;
        }
    }
}