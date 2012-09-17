namespace System.Collections.Generic
{
	public class SkipListNode<TKey, TValue>
	{
		#region Globals

		private TKey thisKey;
		private TValue thisValue;
		private SkipListNode<TKey, TValue> rightNode, downNode;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SkipListNode&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		internal SkipListNode()	{}

		/// <summary>
		/// Initializes a new instance of the <see cref="SkipListNode&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="val">The value.</param>
		internal SkipListNode(TKey key, TValue val)
		{
			thisKey = key;
			thisValue = val;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		internal TKey Key
		{
			get
			{
				return thisKey;
			}
			set
			{
				thisKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		internal TValue Value
		{
			get
			{
				return thisValue;
			}
			set
			{
				thisValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the right node.
		/// </summary>
		/// <value>The right node.</value>
		internal SkipListNode<TKey, TValue> Right
		{
			get
			{
				return rightNode;
			}
			set
			{
				rightNode = value;
			}
		}

		

		/// <summary>
		/// Gets or sets the down node.
		/// </summary>
		/// <value>The down node.</value>
		internal SkipListNode<TKey, TValue> Down
		{
			get
			{
				return downNode;
			}
			set
			{
				downNode = value;
			}
		}

		#endregion
	}
}
