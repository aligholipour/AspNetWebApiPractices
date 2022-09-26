﻿using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Models.Customers;

namespace AspNetWebApiPractices.Services.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomerById(int customerId)
        {
            return _context.Customers.Find(customerId);
        }

        public List<CustomerDto> GetCustomers()
        {
            return _context.Customers.Select(x => new CustomerDto
            {
                Id = x.Id,
                FullName = x.FullName
            }).ToList();
        }

        public void CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }
    }
}