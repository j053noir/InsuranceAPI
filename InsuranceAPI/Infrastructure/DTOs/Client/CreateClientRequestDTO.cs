﻿using InsuranceAPI.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAPI.Infrastructure.DTOs.Client
{
    public class CreateClientRequestDTO
    {
        [Required, EnumDataType(typeof(ClientRole))]
        public ClientRole? Role { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string Identification { get; set; }

        [Required]
        public string IdentificationType { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, EmailAddress]
        public string CofirmEmail { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string ConfirmPhone { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
    }
}
