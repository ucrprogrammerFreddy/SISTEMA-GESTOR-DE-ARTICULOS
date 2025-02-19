﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppUsuarios.Models
{
    public class Articulos
    {
        [Key]
        public int IdArticulo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        public string? ImagenURL { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("[A-Z]", ErrorMessage = "El estado debe ser un único carácter en mayúscula.")]
        public char Estado { get; set; }

        //  Relación con Categoría (Un Artículo pertenece a una Categoría)
       public int IdCategoria { get; set; }
        public Categorias Categoria { get; set; }

        // Relación Muchos a Muchos con Usuarios (Autores)
        public ICollection<ArticuloAutor> ArticuloAutor { get; set; } = new List<ArticuloAutor>();

        // Relación con Etiquetas
         public ICollection<Etiquetas > Etiqueta { get; set;}


    }
}