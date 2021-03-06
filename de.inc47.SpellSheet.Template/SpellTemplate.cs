using System;
using System.Collections.Generic;
using de.inc47.Spells;
using de.inc47.Spells.Enumerations;
using de.inc47.SpellSheet.Render;
using de.inc47.SpellSheet.Render.Enum;

namespace de.inc47.SpellSheet.Template
{
  public class SpellTemplate : ISpellTemplate
  {
    private readonly int AvailableRows = 56;
    private readonly int AvailableColumns = 37;

    public IBlock Apply(ISpell spell, ICharacterInformation info)
    {
      var root = new Block("RootBlock");

      root.Children.Add(RenderEigenschaften(info));
      root.Children.Add(RenderSpellName(spell.Name));
      root.Children.Add(RenderProbe(spell, info));
      root.Children.Add(RenderZfW(spell.ZfW));
      root.Children.Add(RenderZauberdauer(spell));
      root.Children.Add(RenderReichweite(spell));
      return root;
    }

    private IRenderable RenderReichweite(ISpell spell)
    {
      var block = new Block("Reichweite");
      var label = new Text(15, 0, 5, 1, "Reichweite:", TextStyle.Label);
      var content = new Text(15, 5, 4, 1, string.Format("{0} {1}", spell.Reichweite, spell.ReichweiteEinheit), TextStyle.Default);
      block.Children.Add(label);
      block.Children.Add(content);
      return block;
    }

    private IRenderable RenderZauberdauer(ISpell spell)
    {
      var block = new Block("ZD");
      var label = new Text(8, 4, 5, 2, "Zauberdauer:", TextStyle.Label);
      var zauberDauerText = new Text(8, 9, 24, 1, string.Format("{0} {1}", spell.ZD, spell.ZDEinheit), TextStyle.Default);
      block.Children.Add(label);
      block.Children.Add(zauberDauerText);
      var dauer = new Numeric(9, 9, 24, 1, NumericStyle.Boxes, spell.ZD);
      block.Children.Add(dauer);
      return block;
    }

    private IRenderable RenderProbe(ISpell spell, ICharacterInformation info)
    {
      IBlock b = new Block("ProbeBlock");
      string probe = string.Format("{0}/{1}/{2}", spell.Probe1.ToString(), spell.Probe2.ToString(), spell.Probe3.ToString());
      b.Children.Add(new Text(7, AvailableColumns - 4, 4, 1, probe, TextStyle.Label));
      string probenWerte = string.Format("{0}/{1}/{2}{3}{4}",
        info.GetEigenschaft(spell.Probe1),
        info.GetEigenschaft(spell.Probe2),
        info.GetEigenschaft(spell.Probe3),
        spell.ZfW > 0 ? "+" : "-",
        spell.ZfW);
      b.Children.Add(new Text(8, AvailableColumns - 4, 4, 1, probenWerte, TextStyle.Default));
      return b;
    }

    private IRenderable RenderSpellName(string spellName)
    {
      return new Text(0, 0, 37, 3, spellName, TextStyle.Header);
    }

    private Block RenderEigenschaften(ICharacterInformation character)
    {
      var block = new Block("EigenschaftenBlock");
      int columnOffset = 0;
      int width = 3;
      foreach (Eigenschaft e in Enum.GetValues(typeof(Eigenschaft)))
      {
        var eigenschaftsBlock = new Block(string.Format("Eigenschaft{0}Block", e.ToString()));
        var label = new Text(54, 6 + columnOffset, width, 1, e.ToString(), TextStyle.Label);
        var text = new Text(55, 6 + columnOffset, width, 1, character.GetEigenschaft(e).ToString(), TextStyle.Default);
        eigenschaftsBlock.Children.Add(label);
        eigenschaftsBlock.Children.Add(text);
        block.Children.Add(eigenschaftsBlock);
        columnOffset += width;
      }
      return block;
    }

    private IBlock RenderZfW(int zfw)
    {
      var text = new Text(3, AvailableColumns - 4, 4, 4, string.Format("+{0}", zfw), TextStyle.Header);
      return new Block("ZfW", new List<IRenderable> { text });
    }
  }
}