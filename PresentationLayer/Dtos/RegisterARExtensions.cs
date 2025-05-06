using DataAccessLayer.Entities;
using RentingCars.Models.Auth.ActionRequest;

public static class RegisterARExtensions
{
    public static Customer ToCustomer(this RegisterAR registerAR)
    {
        return new Customer()
        {
            UserName = registerAR.PhoneNumber,
            FullName = registerAR.Name,
            PhoneNumber = registerAR.PhoneNumber,
            DateOfBirth = registerAR.DateOfBirth,
            Gender = registerAR.Gender,
            Address = registerAR.Address
        };
    }
}