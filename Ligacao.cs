using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj4
{
  public class Ligacao : IComparable<Ligacao> 
  {
    Cidade cidadeDestino;
    int distancia;

    public Cidade CidadeDestino
    {
      get => cidadeDestino;
      set => cidadeDestino = value;
    }

    public int Distancia
    {
      get => distancia;
      set => distancia = value;
    }

    public Ligacao(Cidade cidadeDestino, int distancia)
    {
      if (cidadeDestino == null)
        throw new ArgumentNullException(nameof(cidadeDestino), "Cidade de destino não pode ser nula");
      if (distancia < 0)
        throw new ArgumentException("Distância não pode ser negativa", nameof(distancia));
      
      this.cidadeDestino = cidadeDestino;
      this.distancia = distancia;
    }

    public override string ToString()
    {
      if (cidadeDestino == null)
        return "Destino desconhecido";
      return cidadeDestino.Nome.TrimEnd() + " (" + distancia + " km)";
    }

    public int CompareTo(Ligacao other)
    {
      if (other == null)
        return 1;
      if (cidadeDestino == null && other.cidadeDestino == null)
        return 0;
      if (cidadeDestino == null)
        return -1;
      if (other.cidadeDestino == null)
        return 1;
      return cidadeDestino.CompareTo(other.cidadeDestino);
    }
  }
}
