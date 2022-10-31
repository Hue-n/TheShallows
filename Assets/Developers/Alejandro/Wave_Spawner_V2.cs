using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Spawner_V2 : MonoBehaviour
{
    [System.Serializable]
    
    public class WaveAction
    {
        public string name;
        
        public float delay;
        
        public GameObject enemy;
        
        public int spawnCount;
        
        public string message;
    }
 
    [System.Serializable]
    
    public class Wave
    {
        public string name;
        
        public List<WaveAction> actions;
    }

    public Transform[] spawnPoints;
 
    public float difficultyFactor = 0.9f;
    
    public List<Wave> waves;
    
    private Wave m_CurrentWave;
    
    public Wave CurrentWave { get {return m_CurrentWave;} }
    
    private float m_DelayFactor = 1.0f;
 
    IEnumerator SpawnLoop()
    {
        m_DelayFactor = 1.0f;
        
        while(true)
        {
            foreach(Wave W in waves)
            {
                m_CurrentWave = W;
                
                foreach(WaveAction A in W.actions)
                {
                    if(A.delay > 0)
                        
                        yield return new WaitForSeconds(A.delay * m_DelayFactor);
                    
                    if (A.message != "")
                    {
                         // TODO: print ingame message
                    }
                    
                    if (A.enemy != null && A.spawnCount > 0)
                    {
                        for(int i = 0; i < A.spawnCount; i++)
                        {
                            Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];

                            Instantiate (A.enemy, _sp.position, _sp.rotation);
                        }
                    }
                    // if (m_CurrentWave = 5)
                    // {
                    //     WaveAction newWave = new WaveAction(name, delay, obj, spawnCount, message);
                    // }
                }
                
                yield return null;  // prevents crash if all delays are 0
            }
            
            m_DelayFactor *= difficultyFactor;
            
            yield return null;  // prevents crash if all delays are 0
        }
    }
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }
}