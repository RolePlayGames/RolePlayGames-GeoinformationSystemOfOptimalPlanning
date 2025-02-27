﻿using FluentMigrator;
using GSOP.Infrastructure.DataAccess.FilmTypes;
using GSOP.Infrastructure.DataAccess.ProductionLines;
using GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20241125004133)]
public class _20241125004133_ProductionRulesCreated : Migration
{
    public override void Up()
    {
        Create.Table(NozzleChangeRulePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(NozzleChangeRulePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(NozzleChangeRulePOCO.ProductionLineID)).AsInt64().NotNullable()
            .WithColumn(nameof(NozzleChangeRulePOCO.NozzleTo)).AsDouble().NotNullable()
            .WithColumn(nameof(NozzleChangeRulePOCO.ChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(NozzleChangeRulePOCO.ChangeConsumption)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(NozzleChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(NozzleChangeRulePOCO.ProductionLineID))
            .ToTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionLinePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table(CalibrationChangeRulePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(CalibrationChangeRulePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(CalibrationChangeRulePOCO.ProductionLineID)).AsInt64().NotNullable()
            .WithColumn(nameof(CalibrationChangeRulePOCO.CalibrationTo)).AsDouble().NotNullable()
            .WithColumn(nameof(CalibrationChangeRulePOCO.ChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(CalibrationChangeRulePOCO.ChangeConsumption)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(CalibrationChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(CalibrationChangeRulePOCO.ProductionLineID))
            .ToTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionLinePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table(CoolingLipChangeRulePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(CoolingLipChangeRulePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(CoolingLipChangeRulePOCO.ProductionLineID)).AsInt64().NotNullable()
            .WithColumn(nameof(CoolingLipChangeRulePOCO.CoolingLipTo)).AsDouble().NotNullable()
            .WithColumn(nameof(CoolingLipChangeRulePOCO.ChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(CoolingLipChangeRulePOCO.ChangeConsumption)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(CoolingLipChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(CoolingLipChangeRulePOCO.ProductionLineID))
            .ToTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionLinePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table(FilmTypeChangeRulePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(FilmTypeChangeRulePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(FilmTypeChangeRulePOCO.ProductionLineID)).AsInt64().NotNullable()
            .WithColumn(nameof(FilmTypeChangeRulePOCO.FilmTypeFromID)).AsInt64().NotNullable()
            .WithColumn(nameof(FilmTypeChangeRulePOCO.FilmTypeToID)).AsInt64().NotNullable()
            .WithColumn(nameof(FilmTypeChangeRulePOCO.ChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(FilmTypeChangeRulePOCO.ChangeConsumption)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(FilmTypeChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(FilmTypeChangeRulePOCO.ProductionLineID))
            .ToTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionLinePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.ForeignKey()
            .FromTable(FilmTypeChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(FilmTypeChangeRulePOCO.FilmTypeFromID))
            .ToTable(FilmTypePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(FilmTypePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.ForeignKey()
            .FromTable(FilmTypeChangeRulePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(FilmTypeChangeRulePOCO.FilmTypeToID))
            .ToTable(FilmTypePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(FilmTypePOCO.ID))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table(NozzleChangeRulePOCO.TableName).InSchema(DBConstants.Schema);
        Delete.Table(CalibrationChangeRulePOCO.TableName).InSchema(DBConstants.Schema);
        Delete.Table(CoolingLipChangeRulePOCO.TableName).InSchema(DBConstants.Schema);
        Delete.Table(FilmTypeChangeRulePOCO.TableName).InSchema(DBConstants.Schema);
    }
}
