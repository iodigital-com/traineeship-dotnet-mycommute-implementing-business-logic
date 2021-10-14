using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyCommute.Domain.Exceptions;

namespace MyCommute.Domain.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly DataContext dataContext;
        
    public EmployeeService(DataContext context)
    {
        dataContext = context;
    }

    public async Task<IEnumerable<Employee>> GetAsync()
    {
        return await dataContext.Employees.ToListAsync().ConfigureAwait(false);
    }
    
    public async Task<Employee> GetByIdAsync(Guid id)
    {
        var employee = await dataContext.Employees.FirstOrDefaultAsync(x => x.Id.Equals(id)).ConfigureAwait(false);

        if (employee == null)
        {
            throw new EmployeeNotFoundException();
        }

        return employee;
    }

    public async Task<Employee> GetByEmailAsync(string email)
    {
        var employee = await dataContext.Employees.FirstOrDefaultAsync(x => x.Email.Equals(email)).ConfigureAwait(false);

        if (employee == null)
        {
            throw new EmployeeNotFoundException();
        }

        return employee;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        dataContext.Employees.Add(employee);

        await dataContext.SaveChangesAsync().ConfigureAwait(false);
        
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var dbEmployee = await GetByIdAsync(employee.Id);
        
        dataContext.Entry(dbEmployee).CurrentValues.SetValues(employee);
        
        await dataContext.SaveChangesAsync().ConfigureAwait(false);
        
        return dbEmployee;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var dbEmployee = await GetByIdAsync(id).ConfigureAwait(false);

        dataContext.Employees.Remove(dbEmployee);
        
        return await dataContext.SaveChangesAsync().ConfigureAwait(false) != 0;
    }

    public async Task<bool> DeleteAsync(string email)
    {
        var dbEmployee = await GetByEmailAsync(email).ConfigureAwait(false);

        dataContext.Employees.Remove(dbEmployee);
        
        return await dataContext.SaveChangesAsync().ConfigureAwait(false) != 0;
    }
}