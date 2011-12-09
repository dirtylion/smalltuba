﻿// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SmallTuba.Network.RPC
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A message describing a keyword and a value
    /// </summary>
    [Serializable]
    internal class Message
    {
        /// <summary>
        /// The keyword
        /// </summary>
        private readonly Keyword keyword;

        private string sender;

        /// <summary>
        /// The value
        /// </summary>
        private readonly object value;

        /// <summary>
        /// May I have a new message with this keyword and this value?
        /// </summary>
        /// <param name="keyword">The keyword</param>
        /// <param name="value">The value</param>
        public Message(Keyword keyword, string sender, object value)
        {
            this.keyword = keyword;
            this.sender = sender;
            this.value = value;
        }

        /// <summary>
        /// What is the keyword of this message?
        /// </summary>
        [Pure]
        public Keyword GetKeyword
        {
            get
            {
                return this.keyword;
            }
        }

        public string GetSender
        {
            get
            {
                return sender;
            }
        }
        
        /// <summary>
        /// What is the value of this message?
        /// </summary>
        [Pure]
        public object GetValue
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// What is the textural representation of this message?
        /// </summary>
        /// <returns>The textual representation</returns>
        [Pure]
        public override string ToString()
        {
            return this.keyword + "," + this.sender + "," + this.value;
        }
    }
}