using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CombatAnalysis.Parser.Tests.CombatParser;

[TestFixture]
internal class CombatDetailsExtensionTests
{
    private Mock<ILogger> _loggerMock;
    private CombatDetails _combatDetails;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger>();
        _combatDetails = new CombatDetails(_loggerMock.Object);
    }

    [Test]
    public void CalculateGeneralData_ShouldNotAddNewItems_WhenPlayersIdIsNull()
    {
        _combatDetails.CalculateGeneralData(null, "00:10:00");

        Assert.Multiple(() =>
        {
            Assert.That(_combatDetails.DamageDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.HealDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.DamageTakenGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.ResourcesRecoveryGeneral.Count, Is.Zero);
        });
    }

    [Test]
    public void CalculateGeneralData_ShouldNotAddNewItems_WhenPlayersIdIsEmpty()
    {
        _combatDetails.CalculateGeneralData([], "00:10:00");

        Assert.Multiple(() =>
        {
            Assert.That(_combatDetails.DamageDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.HealDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.DamageTakenGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.ResourcesRecoveryGeneral.Count, Is.Zero);
        });
    }

    [Test]
    public void CalculateGeneralData_ShouldNotAddNewItems_WhenDurationIsNull()
    {
        _combatDetails.CalculateGeneralData(["player1"], null);

        Assert.Multiple(() =>
        {
            Assert.That(_combatDetails.DamageDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.HealDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.DamageTakenGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.ResourcesRecoveryGeneral.Count, Is.Zero);
        });
    }

    [Test]
    public void CalculateGeneralData_ShouldNotAddNewItems_WhenDurationIsEmpty()
    {
        _combatDetails.CalculateGeneralData(["player1"], "");

        Assert.Multiple(() =>
        {
            Assert.That(_combatDetails.DamageDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.HealDoneGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.DamageTakenGeneral.Count, Is.Zero);
            Assert.That(_combatDetails.ResourcesRecoveryGeneral.Count, Is.Zero);
        });
    }

    [Test]
    public void CalculateGeneralData_ShouldCalculateData_WhenInputsAreValid()
    {
        var playersId = new List<string> { "player1" };
        var duration = "00:10:00";

        _combatDetails.DamageDone.TryAdd("player1", [new DamageDone { Spell = "Spell1", Value = 100 }]);
        _combatDetails.HealDone.TryAdd("player1", [new HealDone { Spell = "Spell1", Value = 100 }]);
        _combatDetails.DamageTaken.TryAdd("player1", [new DamageTaken { Spell = "Spell1", Value = 100 }]);
        _combatDetails.ResourcesRecovery.TryAdd("player1", [new ResourceRecovery { Spell = "Spell1", Value = 100 }]);

        _combatDetails.CalculateGeneralData(playersId, duration);

        Assert.Multiple(() =>
        {
            Assert.That(_combatDetails.DamageDoneGeneral, Is.Not.Empty);
            Assert.That(_combatDetails.HealDoneGeneral, Is.Not.Empty);
            Assert.That(_combatDetails.DamageTakenGeneral, Is.Not.Empty);
            Assert.That(_combatDetails.ResourcesRecoveryGeneral, Is.Not.Empty);
        });
    }
}
