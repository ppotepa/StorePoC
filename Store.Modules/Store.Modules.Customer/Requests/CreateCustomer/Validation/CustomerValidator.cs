﻿using FluentValidation;
using FluentValidation.Results;
using Store.Domain.Entities;
using Serilog;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Store.Modules.Customers.Requests.CreateCustomer
{
    /// <summary>
    /// Customer Validator.
    /// </summary>
    public class CustomerValidator : AbstractValidator<CreateCustomerRequest>
    {
        private readonly ILogger logger;        
        private static readonly Regex SafePasswordRegex 
            = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

        private string SafePasswordMessage => string.Join
        (
            separator: Environment.NewLine,
            value: new string[]
            {
                "Ensure your Password has at least one digit                😍.",
                "Ensure your Password has at least one uppercase letters    😎.",
                "Ensure your Password has at least one lowercase letters    😉.",
                "Ensure your Password length of 8                           😇.",
            }
        );

        public CustomerValidator(ILogger logger)
        {
           logger.Information(nameof(CustomerValidator) + " " + this.GetHashCode());
            
            this.logger = logger;
            this.CascadeMode = CascadeMode.Stop;

            RuleFor(request => request).NotNull();
            RuleFor(request => request.Customer).NotNull();

            When(x => x != null && x.Customer != null, () =>
            {
                RuleFor(request => request.Customer.CompanyName).NotNull().NotEmpty();
                RuleFor(request => request.Customer.Password).NotNull().NotEmpty().WithMessage("Password cannot be null");
                RuleFor(request => request.Customer.Password).Must(BeSafe).WithMessage(SafePasswordMessage);
                RuleFor(request => request.Customer.PasswordConfirmation).NotNull().NotEmpty().WithMessage("Password Confirmation cannot be null");
                RuleFor(request => request.Customer.PasswordConfirmation).Equal(x => x.Customer.Password).WithMessage("Passwords must match.");
                RuleFor(request => request.Customer.Website).Must(ExistIfStringIsNotNull).WithMessage("Invalid website url or website does not exist.");

                RuleForEach(x => x.Customer.Addresses).Must(AllBeValid);
            });
        }

        private bool AllBeValid(Address address)
        {
            //TODO Finish Address Validation.
            return true;
        }

        private bool BeSafe(string passwordString) 
            => SafePasswordRegex.IsMatch(passwordString);

        private bool ExistIfStringIsNotNull(string websiteUrl)
        {
            bool isValidForHttpClientCheck = string.IsNullOrEmpty(websiteUrl) is false && IsValidUrl(websiteUrl);

            if (isValidForHttpClientCheck is true) 
            {
                return new HttpClient().GetAsync(websiteUrl).GetAwaiter().GetResult().IsSuccessStatusCode;
            }
            
            return true;
        }

        private static bool IsValidUrl(string websiteUrl) =>
            Uri.IsWellFormedUriString(websiteUrl, UriKind.RelativeOrAbsolute);

        public override ValidationResult Validate(ValidationContext<CreateCustomerRequest> context)
        {
            logger.Information($"Started validation for : {this.GetType().Name}");
            return base.Validate(context);
        }
    }
}
