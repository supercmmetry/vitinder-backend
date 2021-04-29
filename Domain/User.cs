using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Domain
{
    public enum Sex
    {
        Male,
        Female,
        Other
    }

    public enum SexualOrientation
    {
        Straight,
        Gay,
        Lesbian,
        Bisexual,
        Asexual,
        Demisexual,
        Pansexual,
        Queer,
        Bicurious,
        Aromantic
    }

    public class User
    {
        [Key] public string Id { get; set; }

        [StringLength(256)] [Required] public string FirstName { get; set; }

        [StringLength(256)] [Required] public string LastName { get; set; }
        
        [StringLength(8)] [Required] public String Sex { get; set; }

        [StringLength(32)] [Required] public String SexualOrientation { get; set; }

        [Range(16, 100)] [Required] public int Age { get; set; }

        [StringLength(64)] public string FieldOfStudy { get; set; }

        [Range(1, 16)] public int YearOfStudy { get; set; }

        [StringLength(512)] public string Bio { get; set; }

        [Range(0, 10)] [Required] public int AccessLevel { get; set; }

        public ICollection<Passion> Passions { get; set; }

        public ICollection<Hate> Hates { get; set; }
    }

    public class UserRequest
    {
        [StringLength(256)] [Required] public string FirstName { get; set; }

        [StringLength(256)] [Required] public string LastName { get; set; }

        [Range(16, 100)] [Required] public int Age { get; set; }

        [StringLength(8)] [Required] public String Sex { get; set; }

        [StringLength(32)] [Required] public String SexualOrientation { get; set; }
    }

    public class UserResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public String Sex { get; set; }

        public String SexualOrientation { get; set; }
    }
}