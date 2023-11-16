using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClusterCreator
{
    public class VehiclePositionCluster
    {
        /// <summary>
        /// Get a set of clusters based off of your exsting set that you wish to split by distance.
        /// </summary>
        /// <param name="clusterList"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public Dictionary<int, BaseCluster> GetClusters(List<BaseCluster> clusterList, double maxDistance)
        {           
            //CLUSTER THE DATA
            return ClusterTheData(clusterList, maxDistance);
        }

        /// <summary>
        /// Froms a new set of clusters based off an base list.
        /// </summary>
        /// <param name="clusterList">Existing list of base clusters.</param>
        /// <param name="maxDistance">The max distance each cluster should be apart.</param>
        /// <returns>A cluster set based off an exsisting cluster set, split by max range.</returns>
        private Dictionary<int, BaseCluster> ClusterTheData(List<BaseCluster> clusterList, double maxDistance)
        {
            //CLUSTER DICTIONARY
            var clusterDictionary = new Dictionary<int, BaseCluster>();

            //Add the first node to the cluster list
            if (clusterList.Count > 0)
            {
                clusterDictionary.Add(clusterList[0].Id, clusterList[0]);
            }

            //ALGORITHM
            for (int i = 1; i < clusterList.Count; i++)
            {
                BaseCluster combinedCluster = null;
                BaseCluster oldCluster = null;
                foreach (var clusterDict in clusterDictionary)
                {
                    //Check if the current item belongs to any of the existing clusters
                    if (CheckIfInCluster(clusterDict.Value, clusterList[i], maxDistance))
                    {
                        //If it belongs to the cluster then combine them and copy the cluster to oldCluster variable;
                        combinedCluster = CombineClusters(clusterDict.Value, clusterList[i]);
                        oldCluster = new BaseCluster(clusterDict.Value);
                    }
                }

                //This check means that no suitable clusters were found to combine, so the current item in the list becomes a new cluster.
                if (combinedCluster == null)
                {
                    //Adding new cluster to the cluster dictionary 
                    clusterDictionary.Add(clusterList[i].Id, clusterList[i]);
                }
                else
                {
                    //We have created a combined cluster. Now it is time to remove the old cluster from the dictionary and instead of it add a new cluster.
                    clusterDictionary.Remove(oldCluster.Id);
                    clusterDictionary.Add(combinedCluster.Id, combinedCluster);
                }
            }
            return clusterDictionary;
        }

        /// <summary>
        /// Combine the base cluster with, the new cluster; creating a new base
        /// </summary>
        /// <param name="baseCluster">The base cluster to work the lat, lon from.</param>
        /// <param name="exsistingCluster">The exsisting cluster to merge into base cluser</param>
        /// <returns> New cluster that is the combination of base and another exsisting cluster.</returns>
        private static BaseCluster CombineClusters(BaseCluster baseCluster, BaseCluster exsistingCluster)
        {
            //Deep copy of the home object
            var combinedCluster = new BaseCluster(baseCluster);
            combinedCluster.LatLonList.AddRange(exsistingCluster.LatLonList);
            combinedCluster.Registrations.AddRange(exsistingCluster.Registrations);            

            //Recalibrate the new center
            combinedCluster.LatLonCenter = new LatLong
            {
                Latitude = ((baseCluster.LatLonCenter.Latitude + exsistingCluster.LatLonCenter.Latitude) / 2.0),
                Longitude = ((baseCluster.LatLonCenter.Longitude + exsistingCluster.LatLonCenter.Longitude) / 2.0)
            };

            return combinedCluster;
        }
        
        /// <summary>
        /// Takes a singel set of lat, lon as a cluster and checks if it matches the grouped cluster, based on max distance away from center
        /// </summary>
        /// <param name="startCluster">your lat, lon being a singular entry cluster</param>
        /// <param name="endCluster">the custer containing all lat lon's that is grouped.</param>
        /// <param name="maxDistanceFromCenter">How far from the grouped center do you want to validate?</param>
        /// <returns>If the cluster proposed falls within the custer that is grouped</returns>
        internal bool CheckIfInCluster(BaseCluster startCluster, BaseCluster endCluster, double maxDistanceFromCenter)
        {
            foreach (var startCoordinates in startCluster.LatLonList)
            {
                var startCoord = new GeoCoordinate(startCoordinates.Latitude, startCoordinates.Longitude);
                var endCoordCenter = new GeoCoordinate(endCluster.LatLonCenter.Latitude, endCluster.LatLonCenter.Longitude);
                var distance = startCoord.GetDistanceTo(endCoordCenter);
                if (distance <= maxDistanceFromCenter)
                    return true;
            }
            return false;
        }        
    }
}
