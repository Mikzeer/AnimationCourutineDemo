public static class Helper
{
    public static float GetPercent(float current, float maximum)
    {
        float percent = (current / maximum) * 100;

        return percent;
    }
}

