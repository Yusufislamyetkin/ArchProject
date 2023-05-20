using Arch.BussinessLayer.Abstract;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer GetCustomerById(int customerId)
        {
            // Gerekli iş mantığı ve veritabanı işlemlerini gerçekleştirin
            // Örneğin:
            return _customerRepository.Get(c => c.Id == customerId);
        }

        public List<Customer> GetAllCustomers()
        {
            // Gerekli iş mantığı ve veritabanı işlemlerini gerçekleştirin
            // Örneğin:
            return _customerRepository.List();
        }

        public void CreateCustomer(Customer customer)
        {
            // Gerekli iş mantığı ve veritabanı işlemlerini gerçekleştirin
            // Örneğin:
            _customerRepository.Insert(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            // Gerekli iş mantığı ve veritabanı işlemlerini gerçekleştirin
            // Örneğin:
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            // Gerekli iş mantığı ve veritabanı işlemlerini gerçekleştirin
            // Örneğin:
            var customer = _customerRepository.Get(c => c.Id == customerId);
            if (customer != null)
            {
                _customerRepository.Delete(customer);
            }
        }
    }
}
