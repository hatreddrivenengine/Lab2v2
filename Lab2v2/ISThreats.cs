using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2v2
{
    public class ISThreat
    {
        public int ThreatId { get; set; }
        public string ThreatInfo { get; set; }
        public string ThreatName { get; set; }
        public string ThreatSource { get; set; }
        public string ThreaObj { get; set; }
        public bool ConfidentialityViolation { get; set; }
        public bool IntegrityViolation { get; set; }
        public bool AvailabilityViolation { get; set; }
        public ISThreat(int threatId, string threatInfo, string threatName, string threatSource, string threatObj, bool cnfdVoil, bool intgrtViol, bool avlbltViol)
        {
            this.ThreatId = threatId;
            this.ThreatInfo = threatInfo;
            this.ThreatName = threatName;
            this.ThreatSource = threatSource;
            this.ThreaObj = threatObj;
            this.ConfidentialityViolation = cnfdVoil;
            this.IntegrityViolation = intgrtViol;
            this.AvailabilityViolation = avlbltViol;
        }
        public ISThreat(int i)
        {
            this.ThreatId = i;
        }

        public override bool Equals(object obj)
        {
            return obj is ISThreat threat &&
                   ThreatId == threat.ThreatId &&
                   ThreatInfo == threat.ThreatInfo &&
                   ThreatName == threat.ThreatName &&
                   ThreatSource == threat.ThreatSource &&
                   ThreaObj == threat.ThreaObj &&
                   ConfidentialityViolation == threat.ConfidentialityViolation &&
                   IntegrityViolation == threat.IntegrityViolation &&
                   AvailabilityViolation == threat.AvailabilityViolation;
        }

        public override int GetHashCode()
        {
            int hashCode = 1952433015;
            hashCode = hashCode * -1521134295 + ThreatId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ThreatInfo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ThreatName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ThreatSource);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ThreaObj);
            hashCode = hashCode * -1521134295 + ConfidentialityViolation.GetHashCode();
            hashCode = hashCode * -1521134295 + IntegrityViolation.GetHashCode();
            hashCode = hashCode * -1521134295 + AvailabilityViolation.GetHashCode();
            return hashCode;
        }
    }
}
