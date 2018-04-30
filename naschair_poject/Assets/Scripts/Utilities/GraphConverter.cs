using UnityEngine;

public class GraphConverter : MonoBehaviour {

    //Collects a select amount of samples from over a graph.
    //Exports samples along with the time intervals between each point of data.

    [Range(5, 200)]
    public int sampleAmount;

    public float[] samples; //Array of all samples.
    public float interval; //Time between each sample value.

    public AnimationCurve curve; //The curve which is being converted to values.

    private void Awake()
    {
        if(curve != null)
            StartCalculation();
    }

    public void StartCalculation()
    {
        if (curve.length > 1)
        {
            interval = CalculateIntervalTime();
            samples = GetData(interval);
        }
        else
        {
            Debug.Break();
            Debug.LogError("your curve needs to have at least two keyframes for this script to work!");
        }
    }

    private float CalculateIntervalTime ()
    {
        //Determines the time interval between segments of data in the curve.
        Keyframe lastKey = curve[curve.length - 1]; //Finds the end point of the curve.
        float length = lastKey.time; //Determines the time of that end point. this is the total duration of the curve.
        float sampleInterval = length / sampleAmount; //Divides the duration by the number of samples being taken.
        return sampleInterval; //Returns the time inbetween each sample.
    }

    private float[] GetData(float intervalTime)
    {
        float[] data = new float[sampleAmount]; //Creates an array with a length equal to the amount of samples.
        float currentCurveTime = intervalTime; //currentCurveTime tracks what point in the curve is currently being extracted.

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = curve.Evaluate(currentCurveTime); //Extracts the data from current position in the curve.
            currentCurveTime += intervalTime; //Moves our position in the graph along to the next point.
        }
        return data;
    }

    public float[] ExtractCurveSamples()
    {
        return samples;
    }

    public float ExtractIntervalTime()
    {
        return interval;
    }
}
