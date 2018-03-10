using QnAMakerBot.AgentModule.Interface;
using QnAMakerBot.AgentModule.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace QnAMakerBot.AgentModule
{
    /// <inheritdoc/>
    public class InMemoryAgentStore : IAgentProvider
    {
        private static readonly object ObjectLock = new object();
        private ConcurrentDictionary<string, Agent> _availableAgents = new ConcurrentDictionary<string, Agent>();

        /// <inheritdoc/>
        public Agent GetNextAvailableAgent()
        {
            Agent agent;
            lock (ObjectLock)
            {
                if (_availableAgents.Count <= 0) return null;

                var key = _availableAgents.Keys.First();
                agent = RemoveAgent(key);
            }
            return agent;
        }

        /// <inheritdoc/>
        public bool AddAgent(Agent agent)
        {
            try
            {
                lock (ObjectLock)
                {
                    _availableAgents.AddOrUpdate(agent.AgentId, agent, (k, v) => agent);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public Agent RemoveAgent(Agent agent)
        {
            lock (ObjectLock)
            {
                return RemoveAgent(agent.AgentId);
            }
        }

        /// <summary>從可用人員池移除一位指定的服務人員</summary>
        /// <param name="id">服務人員識別碼</param>
        /// <returns></returns>
        private Agent RemoveAgent(string id)
        {
            _availableAgents.TryRemove(id, out var resource);
            return resource;
        }
    }
}
