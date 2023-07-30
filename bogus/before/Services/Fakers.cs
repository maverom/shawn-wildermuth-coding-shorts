﻿namespace FakingIt.Services;

using Bogus;
using FakingIt.Entities;

public class Fakers
{
    Faker<Customer>? _customerFaker = null;
    Faker<Address>? _addressFaker = null;

    public Faker<Customer> GetCustomerGenerator(bool includeAddresses = true)
    {
        if (_customerFaker is null)
        {
            var addressFaker = GetAddressGenerator();
            var id = 0;

            _customerFaker = new Faker<Customer>()
                .UseSeed(1969)
                .RuleFor(c => c.Id, f => ++id)
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.ContactName, f => f.Name.FullName())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f));

            if (includeAddresses)
            {
                _customerFaker = _customerFaker
                    .RuleFor(c => c.Address, f => addressFaker.Generate(1).First().OrNull(f, .1f));
            }
        }

        return _customerFaker;
    }

    public Faker<Address> GetAddressGenerator()
    {
        if (_addressFaker is null)
        {
            var id = 0;

            _addressFaker = new Faker<Address>()
                .UseSeed(1969)
                .RuleFor(a => a.Id, f => ++id)
                .RuleFor(a => a.Address1, f => f.Address.StreetAddress())
                .RuleFor(a => a.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.StateProvince, f => f.Address.State())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());
        }

        return _addressFaker;
    }
}
