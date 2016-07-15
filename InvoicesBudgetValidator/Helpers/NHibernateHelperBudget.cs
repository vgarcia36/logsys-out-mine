using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using InvoicesBudgetValidator.Maps;
using InvoicesBudgetValidator.Model;
using NHibernate;

namespace InvoicesBudgetValidator.Helpers
{
    class NHibernateHelperBudget
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
   ("NHibernateHelperBudget");



        private static ISessionFactory _sessionFactory;


        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)

                    InitializeSessionFactory();
                return _sessionFactory;
            }

        }

        private static void InitializeSessionFactory()
        {
            try
            {
                _sessionFactory = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
              .ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringPresupuesto")).ShowSql// Modify your ConnectionString

            )
            .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<BudgetMap>())
            .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<BudgetPartyMap>())
            .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<ConsolidadoMap>())
            .BuildSessionFactory();
            }

            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

        }


        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
