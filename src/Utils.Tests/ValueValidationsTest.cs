using NUnit.Framework;
using Webamoki.Utils;
using Webamoki.Utils.Testing;

namespace Utils.Tests;

public class ValueValidationsTest
{
    [Test]
    public void CheckValue_Null_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check(null));
        Ensure.True(ValueValidations.Check(null, allowNull: true));
    }

    [Test]
    public void CheckValue_GlobalCSS_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("inherit"));
        Ensure.Throws(() => ValueValidations.Check("initial"));
        Ensure.Throws(() => ValueValidations.Check("unset"));
        Ensure.Throws(() => ValueValidations.Check("revert"));
        Ensure.Throws(() => ValueValidations.Check("revert-layer"));
        Ensure.True(ValueValidations.CheckGlobalCSS("inherit", stringPercentage: true));
        Ensure.True(ValueValidations.CheckGlobalCSS("initial", stringInt: true));
        Ensure.True(ValueValidations.CheckGlobalCSS("unset", allowNull: true));
        Ensure.True(ValueValidations.CheckGlobalCSS("revert", stringPercentage: true));
        Ensure.True(ValueValidations.CheckGlobalCSS("revert-layer", stringPercentage: true));
    }

    [Test]
    public void CheckValue_Int_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check(1));
        Ensure.Throws(() => ValueValidations.Check(-1, allowInt: true));
        Ensure.True(ValueValidations.Check(1, allowInt: true));
        Ensure.True(ValueValidations.Check(1, allowDouble: true));
        Ensure.True(ValueValidations.Check(0, allowDouble: true));
        Ensure.True(ValueValidations.Check(0, allowInt: true));
        Ensure.True(ValueValidations.Check(-1, allowInt: true, allowNegative: true));
        Ensure.True(ValueValidations.Check(-1, allowDouble: true, allowNegative: true));
    }

    [Test]
    public void CheckValue_Double_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check(0.1));
        Ensure.Throws(() => ValueValidations.Check(0));
        Ensure.Throws(() => ValueValidations.Check(1.1));
        Ensure.Throws(() => ValueValidations.Check(-1.1, allowDouble: true));
        Ensure.True(ValueValidations.Check(0.0, allowDouble: true));
        Ensure.True(ValueValidations.Check(1.0, allowDouble: true));
        Ensure.True(ValueValidations.Check(10.1, allowDouble: true));
        Ensure.True(ValueValidations.Check(.1, allowDouble: true));
        Ensure.True(ValueValidations.Check(100.01, allowDouble: true));
        Ensure.True(ValueValidations.Check(7.1, allowDouble: true));
        Ensure.True(ValueValidations.Check(0, allowDouble: true));
        Ensure.True(ValueValidations.Check(-7.1, allowDouble: true, allowNegative: true));
    }

    [Test]
    public void CheckValue_StringInt_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("13.0", stringInt: true));
        Ensure.Throws(() => ValueValidations.Check("13.", stringInt: true));
        Ensure.Throws(() => ValueValidations.Check("-13", stringInt: true));
        Ensure.True(ValueValidations.Check("10", stringInt: true));
        Ensure.True(ValueValidations.Check("1", stringInt: true));
        Ensure.True(ValueValidations.Check("201", stringInt: true));
        Ensure.True(ValueValidations.Check("-13", stringInt: true, allowNegative: true));
        Ensure.True(ValueValidations.Check("0", stringInt: true));
    }

    [Test]
    public void CheckValue_StringDouble_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("13.0", stringDouble: true));
        Ensure.Throws(() => ValueValidations.Check("0", stringDouble: true));
        Ensure.Throws(() => ValueValidations.Check("13.10", stringDouble: true));
        Ensure.Throws(() => ValueValidations.Check("13.", stringDouble: true));
        Ensure.Throws(() => ValueValidations.Check("-13.01", stringDouble: true));
        Ensure.True(ValueValidations.Check("10", stringDouble: true));
        Ensure.True(ValueValidations.Check("1", stringDouble: true));
        Ensure.True(ValueValidations.Check("201", stringDouble: true));
        Ensure.True(ValueValidations.Check("13.12", stringDouble: true));
        Ensure.True(ValueValidations.Check("-13.12", stringDouble: true, allowNegative: true));
    }

    [Test]
    public void CheckValue_StringPercentage_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("0%", stringPercentage: true));
        Ensure.Throws(() => ValueValidations.Check("13.0%", stringPercentage: true));
        Ensure.Throws(() => ValueValidations.Check("13.%", stringPercentage: true));
        Ensure.Throws(() => ValueValidations.Check("-13.12%", stringPercentage: true));
        Ensure.True(ValueValidations.Check("10%", stringPercentage: true));
        Ensure.True(ValueValidations.Check("1%", stringPercentage: true));
        Ensure.True(ValueValidations.Check("201%", stringPercentage: true));
        Ensure.True(ValueValidations.Check("13.12%", stringPercentage: true));
        Ensure.True(ValueValidations.Check("-13.12%", stringPercentage: true, allowNegative: true));
    }

    [Test]
    public void CheckValue_StringRem_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("0rem", stringRem: true));
        Ensure.Throws(() => ValueValidations.Check("13.0rem", stringRem: true));
        Ensure.Throws(() => ValueValidations.Check("13.rem", stringRem: true));
        Ensure.Throws(() => ValueValidations.Check("-13.1rem", stringRem: true));
        Ensure.True(ValueValidations.Check("10rem", stringRem: true));
        Ensure.True(ValueValidations.Check("1rem", stringRem: true));
        Ensure.True(ValueValidations.Check("201rem", stringRem: true));
        Ensure.True(ValueValidations.Check("13.12rem", stringRem: true));
        Ensure.True(ValueValidations.Check("-13.12rem", stringRem: true, allowNegative: true));
    }

    [Test]
    public void CheckValue_StringPixel_ReturnsTrue()
    {
        Ensure.Throws(() => ValueValidations.Check("0px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("0", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("1px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("10px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("13.0px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("13.px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("0.1px", stringPixel: true));
        Ensure.Throws(() => ValueValidations.Check("-13.1px", stringPixel: true));
        Ensure.True(ValueValidations.Check(".1px", stringPixel: true));
        Ensure.True(ValueValidations.Check("20.1px", stringPixel: true));
        Ensure.True(ValueValidations.Check("13.12px", stringPixel: true));
        Ensure.True(ValueValidations.Check("-13.12px", stringPixel: true, allowNegative: true));
    }
}
