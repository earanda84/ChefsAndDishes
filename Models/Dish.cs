#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace ChefsAndDishes.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }

        // Name of dish
        [Required]
        [Display(Name = "Name of Dish")]
        public string Name { get; set; }

        // Calories
        [Required]
        [Display(Name = "# of Calories")]
        [MoreThanCero]
        public int Calories { get; set; }

        // Tastiness
        [Required]
        [Display(Name = "Tastiness")]
        [BetweenOneAndFive]
        public int Tastiness { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public int ChefId { get; set; }

        // Navigation Property
        public Chef? Creator { get; set; }
    }

    // Validations
    public class MoreThanCeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Evalúa si el valor del objeto es un int, se lo asigne a la variable calories.
            if (value is int calories)
            {
                // Evalúa si las calorías son <= a 0, retorne un resultado de valdiación
                if (calories <= 0)
                {
                    return new ValidationResult("Las calorías deben ser mayor a cero");
                }
            }

            // Retorna una resultado de validación Exitoso-
            return ValidationResult.Success;
        }
    }

    // Validación si este entre 1 y 5
    public class BetweenOneAndFiveAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Evlúa si el valor pasado en el objeto es int, lo asigne a la variable tastiness
            if (value is int tastiness)
            {
                // Evalúa si esta en el reango de 1 y 5
                if (tastiness <= 0 || tastiness > 5)
                {
                    return new ValidationResult("El sabor deber estar entre 1 y 5.");
                }
            }

            return ValidationResult.Success;
        }
    }
}