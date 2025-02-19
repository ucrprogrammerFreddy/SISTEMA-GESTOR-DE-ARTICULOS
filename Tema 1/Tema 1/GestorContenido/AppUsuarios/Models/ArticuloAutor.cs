﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using AppUsuarios.Models;

namespace AppUsuarios.Models { 
    public class ArticuloAutor
    {
        [Key]
       
        public int IdArticuloAutor { get; set; }

    
        public int IdArticulo { get; set; }
       public Articulos Articulo { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }




    }
}