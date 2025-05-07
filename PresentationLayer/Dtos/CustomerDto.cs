using DataAccessLayer.Entities;
using DataAccessLayer.Enums;

namespace RentingCars.Dtos;

public class CustomerDto
{
    public string FullName { get; set; }
    
    public string? ProfilePhoto { get; set; } 
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
}

public static class CustomerExtensionMethold
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto()
        {
            Address = customer.Address,
            DateOfBirth = customer.DateOfBirth,
            FullName = customer.FullName,
            Gender = customer.Gender,
            ProfilePhoto = customer.ProfilePhoto,
            Phone = customer.PhoneNumber
        };
    }
}