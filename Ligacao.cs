using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj4
{
  public class Ligacao : IComparable<Ligacao> 
  {
    private string cidadeDestino;
    private int distancia;

    public string CidadeDestino
    {
      get => cidadeDestino;
      set => cidadeDestino = value;
    }

    public int Distancia
    {
      get => distancia;
      set => distancia = value;
    }

    public Ligacao()
    {
      cidadeDestino = "";
      distancia = 0;
    }

    public Ligacao(string cidadeDestino, int distancia)
    {
      this.cidadeDestino = cidadeDestino;
      this.distancia = distancia;
    }

    public int CompareTo(Ligacao other)
    {
      if (other == null) return 1;
      if (cidadeDestino == null && other.cidadeDestino == null) return 0;
      if (cidadeDestino == null) return -1;
      if (other.cidadeDestino == null) return 1;
      return cidadeDestino.CompareTo(other.cidadeDestino);
    }

    public override string ToString()
    {
      return $"{cidadeDestino} ({distancia} km)";
    }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
        return false;
      
      Ligacao outra = (Ligacao)obj;
      if (cidadeDestino == null && outra.cidadeDestino == null) return true;
      if (cidadeDestino == null || outra.cidadeDestino == null) return false;
      return cidadeDestino == outra.cidadeDestino;
    }

    public override int GetHashCode()
    {
      return cidadeDestino?.GetHashCode() ?? 0;
    }
  }
}
