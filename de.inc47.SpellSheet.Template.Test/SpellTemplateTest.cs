﻿using System;
using System.Linq;
using de.inc47.Spells;
using de.inc47.Spells.Enumerations;
using de.inc47.SpellSheet.Render;
using de.inc47.SpellSheet.Render.Enum;
using Moq;
using NUnit.Framework;

namespace de.inc47.SpellSheet.Template.Test
{
  public class SpellTemplateTest
  {
    [Test]
    public void TestApplyReichweite()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();
      spellMock.Setup(m => m.Reichweite).Returns(7).Verifiable();
      spellMock.Setup(m => m.ReichweiteEinheit).Returns(DistanzEinheit.Schritt).Verifiable();

      ISpellTemplate sut = new SpellTemplate();
      IBlock block = sut.Apply(spellMock.Object, characterMock.Object);

      Assert.IsTrue(block.ContainsChild(r => r.Id == "Reichweite"));
      IBlock reichweiteBlock = block.FindChild<IBlock>(b => b.Id == "Reichweite");
      IText label = reichweiteBlock.Children.OfType<IText>().FirstOrDefault(r => r.Style == TextStyle.Label);
      Assert.NotNull(label);
      Assert.AreEqual("Reichweite:", label.Content);
      IText content = reichweiteBlock.Children.OfType<IText>().FirstOrDefault(r => r.Style != TextStyle.Label);
      Assert.NotNull(content);
      Assert.AreEqual("7 Schritt", content.Content);
    }

    [Test]
    public void TestApplyZauberdauer()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();
      spellMock.Setup(m => m.ZD).Returns(7).Verifiable();
      spellMock.Setup(m => m.ZDEinheit).Returns(Zeiteinheit.AR).Verifiable();

      ISpellTemplate sut = new SpellTemplate();
      IBlock block = sut.Apply(spellMock.Object, characterMock.Object);

      spellMock.Verify(m => m.ZD, Times.AtLeastOnce);
      spellMock.Verify(m => m.ZDEinheit, Times.Once);
      Assert.IsTrue(block.ContainsChild(r => r.Id == "ZD"));
    }

    [Test]
    public void TestApplyZfW()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();
      spellMock.Setup(m => m.ZfW).Returns(15).Verifiable();

      ISpellTemplate sut = new SpellTemplate();
      IBlock block = sut.Apply(spellMock.Object, characterMock.Object);

      spellMock.Verify(m => m.ZfW, Times.AtLeastOnce);
      Assert.IsTrue(block.ContainsChild(r => r.Id == "ZfW"));
    }

    [Test]
    public void TestApplyGetEigenschaften()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();
      characterMock.Setup(c => c.GetEigenschaft(It.IsAny<Eigenschaft>())).Returns(0).Verifiable();

      var sut = new SpellTemplate();
      var iblock = sut.Apply(spellMock.Object, characterMock.Object);

      foreach (Eigenschaft e in Enum.GetValues(typeof(Eigenschaft)))
      {
        characterMock.Verify(c => c.GetEigenschaft(e), Times.AtLeastOnce);
      }
    }

    [Test]
    public void TestApplyGetSpellName()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();
      spellMock.Setup(s => s.Name).Returns("SpellName").Verifiable();

      var sut = new SpellTemplate();
      var iblock = sut.Apply(spellMock.Object, characterMock.Object);

      spellMock.Verify(s => s.Name, Times.Once);
    }

    [Test]
    public void TestApplyProbe()
    {
      var spellMock = new Mock<ISpell>();
      spellMock.Setup(s => s.Probe1).Returns(Eigenschaft.CH).Verifiable();
      spellMock.Setup(s => s.Probe2).Returns(Eigenschaft.KL).Verifiable();
      spellMock.Setup(s => s.Probe3).Returns(Eigenschaft.FF).Verifiable();
      var characterMock = new Mock<ICharacterInformation>();
      characterMock.Setup(c => c.GetEigenschaft(Eigenschaft.CH)).Returns(14).Verifiable();
      characterMock.Setup(c => c.GetEigenschaft(Eigenschaft.KL)).Returns(15).Verifiable();
      characterMock.Setup(c => c.GetEigenschaft(Eigenschaft.FF)).Returns(12).Verifiable();

      var sut = new SpellTemplate();
      var iblock = sut.Apply(spellMock.Object, characterMock.Object);

      spellMock.Verify(s => s.Probe1, Times.AtLeastOnce);
      spellMock.Verify(s => s.Probe2, Times.AtLeastOnce);
      spellMock.Verify(s => s.Probe3, Times.AtLeastOnce);

      characterMock.Verify(c => c.GetEigenschaft(Eigenschaft.CH), Times.AtLeastOnce);
      characterMock.Verify(c => c.GetEigenschaft(Eigenschaft.KL), Times.AtLeastOnce);
      characterMock.Verify(c => c.GetEigenschaft(Eigenschaft.FF), Times.AtLeastOnce);
    }

    [Test]
    public void TestApplyAllBlocksNamed()
    {
      var spellMock = new Mock<ISpell>();
      var characterMock = new Mock<ICharacterInformation>();

      ISpellTemplate sut = new SpellTemplate();
      var iblock = sut.Apply(spellMock.Object, characterMock.Object);

      bool AssertBlockIdsRecursively(IBlock block)
      {
        return !string.IsNullOrEmpty(block.Id) && block.Children.OfType<IBlock>().All(AssertBlockIdsRecursively);
      }

      Assert.IsTrue(AssertBlockIdsRecursively(iblock));
    }
  }
}
