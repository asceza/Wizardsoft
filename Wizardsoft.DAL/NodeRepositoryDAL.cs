﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.DAL
{
    public class NodeRepositoryDAL : INodeRepository
    {
        private readonly List<Node> _nodes = new();

        /// <summary>
        /// Получить все узлы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetAll() => _nodes;


        /// <summary>
        /// Получить узел по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node GetById(int id) => _nodes.FirstOrDefault(n => n.Id == id);


        /// <summary>
        /// Создать узел
        /// </summary>
        /// <param name="node"></param>
        public int Create(Node node)
        {
            node.Id = _nodes.Count + 1;
            _nodes.Add(node);
            return node.Id;
        }


        /// <summary>
        /// Обновить узел
        /// </summary>
        /// <param name="node"></param>
        public Node Update(Node node)
        {
            var existingNode = GetById(node.Id);
            if (existingNode != null)
            {
                existingNode.Name = node.Name;
                //existingNode.ParentId = node.ParentId;
            }
            return node;
        }


        /// <summary>
        /// Удалить узел по id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var node = GetById(id);
            if (node != null)
            {
                _nodes.Remove(node);
            }
        }
    }

}