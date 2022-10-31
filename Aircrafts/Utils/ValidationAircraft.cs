using System;
using System.Linq;
namespace Aircrafts.Utils
{
    public class ValidationAircraft
    {
        public string RabValidation(string rab)
        {
            rab = rab.ToLower();
            string[] prefixAircraft = new string[] { "pu", "pt", "ps", "pr", "pp", "ph" };
            string[] rabForbidden = new string[] { "sos", "xxx", "pan", "ttt", "vfr", "ifr", "vmc", "imc" };
            char[] letters = rab.ToCharArray();
            //verifica o tamanho do rab
            if (letters.Length == 5)
            {
                //verifica se tem q e w onde não pode ter
                if (letters[2] != 'q' && letters[3] != 'w')
                {
                    string brazilianAeronauticalRegistration = letters[2].ToString().ToLower() + letters[3].ToString().ToLower() + letters[4].ToString().ToLower();
                    if (rabForbidden.Contains(brazilianAeronauticalRegistration) == false)
                    {
                        string prefixRab = letters[0].ToString().ToLower() + letters[1].ToString().ToLower();
                        if (prefixAircraft.Contains(prefixRab) == true)
                        {
                            return rab;
                        }
                        else
                        {
                            return "Prefixos devem ser PU,PT,PS,PR,PP,PH";
                            //  return null;
                        }
                    }
                    else
                    {
                        return "SOS, XXX, PAN, TTT, VFR, IFR, VMC e IMC não podem ser utilizadas";
                        //return null;
                    }
                }
                else
                {
                    return "A letra Q como primeira letra e nem a letra W como segunda letra da matrícula da aeronave não são permitidas";
                    // return null;
                }
            }
            else
            {
                return "Quantidade incorreta de dígitos de identificação";
                // return null;
            }
        }
    }
}