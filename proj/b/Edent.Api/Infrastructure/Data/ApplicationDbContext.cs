using Edent.Api.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private IDbContextTransaction _currentTransaction;
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }


        public DbSet<Address> Addresses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AuthRequest> AuthRequests { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DentalChair> DentalChairs { get; set; }
        public DbSet<DentalService> DentalServices { get; set; }
        public DbSet<DentalServiceCategory> DentalServiceCategories { get; set; }
        public DbSet<DentalServiceGroup> DentalServiceGroups { get; set; }
        public DbSet<DentalServicePrice> DentalServicePrices { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorAddress> DoctorAddresses { get; set; }
        public DbSet<DoctorDentalChair> DoctorDentalChairs { get; set; }
        public DbSet<DoctorInTerm> DoctorInTerms { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<IncomeInventoryItem> IncomeInventoryItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryIncome> InventoryIncomes { get; set; }
        public DbSet<InventoryOutcome> InventoryOutcomes { get; set; }
        public DbSet<InventoryPrice> InventoryPrices { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OutcomeInventoryItem> OutcomeInventoryItems { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientAddress> PatientAddresses { get; set; }
        public DbSet<PatientTooth> PatientTeeth { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Recept> Recepts { get; set; }
        public DbSet<ReceptInventory> ReceptInventories { get; set; }
        public DbSet<ReceptInventorySetting> ReceptInventorySettings { get; set; }
        public DbSet<ReceptInventorySettingItem> ReceptInventorySettingItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleSetting> ScheduleSettings { get; set; }
        public DbSet<SettingDayOfWeek> SettingDayOfWeeks { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Technic> Technics { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Tooth> Teeth { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<MeasurementUnitType> MeasurementUnitTypes { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<DentalServiceReceptInventorySetting> DentalServiceReceptInventorySettings { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<TreatmentDentalService> TreatmentDentalServices { get; set; }
        public DbSet<JointDoctor> JointDoctors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(o => o.Doctor)
                .WithOne(w => w.User)
                .HasForeignKey<Doctor>(d => d.UserId);

            builder.Entity<User>()
                .HasOne(o => o.Employee)
                .WithOne(w => w.User)
                .HasForeignKey<Employee>(d => d.UserId);

            builder.Entity<Doctor>()
               .HasOne(o => o.Schedule)
               .WithOne(w => w.Doctor)
               .HasForeignKey<Schedule>(d => d.DoctorId);

            builder.Entity<Recept>()
                .HasOne(o => o.Appointment)
                .WithOne(o => o.Recept)
                .HasForeignKey<Recept>(r => r.AppointmentId);

            builder.Entity<Invoice>()
                .HasOne(o => o.Recept)
                .WithOne(o => o.Invoice)
                .HasForeignKey<Invoice>(r => r.ReceptId);

        }
    }
}
