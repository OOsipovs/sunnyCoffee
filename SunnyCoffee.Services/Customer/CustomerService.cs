using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SunnyCoffee.Data;

namespace SunnyCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private SunnyDbContext _db;

        public CustomerService(SunnyDbContext sunnyDbContext)
        {
            _db = sunnyDbContext;
        }

        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "New customer created",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
        }

        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            try
            {
                var customer = _db.Customers.Find(id);
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Customer removed",
                    Time = DateTime.UtcNow,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = false
                };
            }
        }

        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(c => c.PrimaryAddress)
                .OrderBy(c => c.LastName)
                .ToList();
        }

        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.FirstOrDefault(c => c.Id == id);
        }
    }
}
