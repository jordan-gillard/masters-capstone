using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Z.EntityFramework.Plus;


namespace Capstone.Models
{

    public partial class SSDEntities : DbContext
    {
        // ... context code ...
        //		public DbSet<AuditEntry> AuditEntries { get; set; }
        //		public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

        public SSDEntities() : base("name=MedicalEntities")
        {
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
            {
                //							var t = context.GetType().GetProperty("Value").GetValue(context, null);
                var type = context.GetType();
                var customAuditEntries = audit.Entries.Select(x => Import(x));
                (context as MedicalEntities).AuditEntries.AddRange(customAuditEntries);
            };
        }
        public AuditEntry Import(Z.EntityFramework.Plus.AuditEntry entry)
        {
            var customAuditEntry = new AuditEntry
            {
                EntitySetName = entry.EntitySetName,
                EntityTypeName = entry.EntityTypeName,
                State = (int)entry.State,
                StateName = entry.StateName,
                CreatedBy = entry.CreatedBy,
                CreatedDate = entry.CreatedDate
            };

            customAuditEntry.AuditEntryProperties = entry.Properties.Select(x => Import(x)).ToList();

            return customAuditEntry;
        }

        public AuditEntryProperty Import(Z.EntityFramework.Plus.AuditEntryProperty property)
        {
            var customAuditEntry = new AuditEntryProperty
            {
                RelationName = property.RelationName,
                PropertyName = property.PropertyName,
                OldValue = property.OldValueFormatted,
                NewValue = property.NewValueFormatted
            };

            return customAuditEntry;
        }

        public override int SaveChanges()
        {
            var audit = new Audit();
            audit.PreSaveChanges(this);
            var rowAffecteds = base.SaveChanges();
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                base.SaveChanges();
            }

            return rowAffecteds;
        }

        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var audit = new Audit();
            audit.PreSaveChanges(this);
            var rowAffecteds = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            return rowAffecteds;
        }
    }


}