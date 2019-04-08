using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.DataAccess.DataLayer
{
    public class WEBPOSInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<WEBPOSContext>
    {
        protected override void Seed(WEBPOSContext context)
        {
            var priceLists = new List<DePriceList>
            {
                new DePriceList{ PriceListCode="01", PriceListDescription="GENERAL"}
            };
            priceLists.ForEach(s => context.PriceLists.Add(s));
            context.SaveChanges();

            var paymentTypes = new List<DePaymentType>
            {
                new DePaymentType{ PaymentTypeCode="EFT", PaymentTypeDescription="EFECTIVO"},
            };
            paymentTypes.ForEach(s => context.PaymentTypes.Add(s));
            context.SaveChanges();

            var unitMeasures = new List<DeUnitMeasure>
            {
                new DeUnitMeasure{ UnitMeasureCode="KG", UnitMeasureDescription="KILOGRAMO"},
                new DeUnitMeasure{ UnitMeasureCode="LB", UnitMeasureDescription="LIBRA"},
                new DeUnitMeasure{ UnitMeasureCode="L", UnitMeasureDescription="LITRO"},
                new DeUnitMeasure{ UnitMeasureCode="FT", UnitMeasureDescription="PIE"},
            };
            unitMeasures.ForEach(s => context.UnitMeasures.Add(s));
            context.SaveChanges();

            var taxes = new List<DeTax>
            {
                new DeTax{ TaxCode="ITBIS", TaxDescription="ITBIS (18%)", TaxPercent=18},
                new DeTax{ TaxCode="NONE", TaxDescription="EXONERADO", TaxPercent=0}
            };
            taxes.ForEach(s => context.Taxes.Add(s));
            context.SaveChanges();

            var depts = new List<DeDepartment>
            {
                new DeDepartment{ DepartmentCode="01", DepartmentDescription="BEBIDAS" },
                new DeDepartment{ DepartmentCode="02", DepartmentDescription="ELECTRONICOS" }
            };
            depts.ForEach(s => context.Departments.Add(s));
            context.SaveChanges();

            var warehouses = new List<DeWarehouse>
            {
                new DeWarehouse{ WarehouseCode="01", WarehouseDescription = "PRINCIPAL" }
            };
            warehouses.ForEach(s => context.Warehouses.Add(s));
            context.SaveChanges();

            var itemWarehouses = new List<DeBusinessPartner>
            {
                new DeBusinessPartner{ BusinessPartnerCode = "01", BusinessPartnerDescription="COCA COLA", BusinessPartnerType = "S" }
            };
            itemWarehouses.ForEach(s => context.BusinessPartners.Add(s));
            context.SaveChanges();

            context.Users.Add(new DeUser { UserCode = "admin", Password = "984sMmyYExc2ccrPpZjbEA==", Gender = Gender.HOMBRE, UserType = UserType.ADMINISTRADOR, IsEditing = true });
            context.SaveChanges();
        }
    }
}