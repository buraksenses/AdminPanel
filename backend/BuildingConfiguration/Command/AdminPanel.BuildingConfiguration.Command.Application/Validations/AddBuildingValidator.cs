using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using FluentValidation;

namespace AdminPanel.BuildingConfiguration.Command.Application.Validations;

public class AddBuildingValidator : AbstractValidator<AddBuildingCommand>
{
    public AddBuildingValidator()
    {
        RuleFor(b => b.BuildingCost)
            .GreaterThan(0).WithMessage("Building cost must be greater than zero!");

        RuleFor(b => b.ConstructionTime)
            .InclusiveBetween(30, 1800)
            .WithMessage("Construction time must be between 30 and 1800!");
    }
}