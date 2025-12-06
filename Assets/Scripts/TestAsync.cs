using UnityEngine;
using System.Threading.Tasks;
public class TestAsync : MonoBehaviour
{
    public async void Start()
    {
        await Task.Delay(3000);
        Debug.Log("Will this run before or after the first update call");
    }
    public async void Update()
    {
        Debug.Log("Update part 1 Will this happen every second, or every frame?");
         await Task.Delay(1000);
        Debug.Log("Update part 2 Will this happen every second, or every frame?");
    }
}
