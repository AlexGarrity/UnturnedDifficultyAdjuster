namespace UnturnedDifficultyAdjuster
{
    public class LimbDamageOverrides
    {
        public static readonly LimbDamageOverrides Default = new LimbDamageOverrides();
        public float Arm;
        public float Body;
        public float Head;
        public float Leg;
        public float Spine;

        public LimbDamageOverrides(float head = 1.1F, float body = 1.0F, float arm = 0.3F, float leg = 0.3F,
            float spine = 0.6F)
        {
            Head = head;
            Body = body;
            Arm = arm;
            Leg = leg;
            Spine = spine;
        }

        public override string ToString()
        {
            return
                $"Head: {Head * 100.0F}%\tBody: {Body * 100.0F}%\tArm: {Arm * 100.0F}%\tLeg: {Leg * 100.0F}%\tSpine: {Spine * 100.0F}%";
        }
    }
}