using System;
using System.Collections;

public class SmartStack<T> : IEnumerable<T>, IEnumerable
{
	private T[] items;
	private int count;

	public int Count => count;
	public int Capacity => items.Length;

	// Конструктор без параметров
	public SmartStack()
	{
		items = new T[4];
		count = 4;
	}
	// Конструктор с заданным параметром
	public SmartStack(int capacity)
	{
		if (capacity <= 0)
			throw new ArgumentOutOfRangeException(nameof(capacity));
		items = new T[capacity];
		count = capacity;
	}
	// Кoнструктор для коллекции
	public SmartStack(IEnumerable<T> collection)
	{
		if (collection == null)
			throw new ArgumentNullException(nameof(collection));
		var list = collection.ToList();
		items = new T[list.Count];
		count = list.Count;
		for (int i = 0; i < list.Count; i++)
		{
			items[i] = list[i];
			count += list.Count;
		}
	}

	// Метод для увеличения длины массива в 2 раза при нехватке
	private void EnsureCapacity(int minCapacity)
	{
		while (items.Length < minCapacity)
		{
			T[] tmpArray = new T[count * 2];
			Array.Copy(items, tmpArray, count);
			items = tmpArray;
		}
		return;
	}

	//Индексатор
	public T this[int depth]
	{
		get
		{
			if ((depth < 0) || (depth > count))
				throw new ArgumentOutOfRangeException(nameof(depth),
					$"Глубина должна быть в диапозоне от 0 до {count - 1}");
			return items[count - 1 - depth];
		}
		set
		{
            if ((depth < 0) || (depth > count))
                throw new ArgumentOutOfRangeException(nameof(depth),
                    $"Глубина должна быть в диапозоне от 0 до {count - 1}");
			items[count - 1 - depth] = value;
        }
	}

	public void Push(T item)
	{
		if (items.Length == count)
		{
			EnsureCapacity(count + 1);
		}
		items[count] = item;
		count++;
	}

	public void PushRange(IEnumerable<T> collection)
	{
		if (collection == null)
			throw new ArgumentNullException(nameof(collection));

		var list = collection.ToList();
		if (list.Count == 0)
			return;

		if (items.Length < list.Count + count)
		{
			EnsureCapacity(list.Count + count);
		}

		for (int i = 0; i < list.Count; i++)
		{
			items[count + i] = list[i];
		}
		count += list.Count;
	}

	public T Pop()
	{
		if (count == 0)
			throw new InvalidOperationException("Стек пуст");
		count--;
		T result = items[count];
		items[count] = default(T);
		return result;
	}

	public T Peek()
	{
		if (count == 0)
			throw new InvalidOperationException("Стек пуст");
		return items[count - 1];
	}

	public bool Contains(T item)
	{
		var comparer = EqualityComparer<T>.Default;

		for (int i = 0; i < count - 1; i++)
		{
			if (comparer.Equals(items[i], item))
				return true;
		}

		return false;
	}

    // Реализация IEnumerable<T> и IEnumerable
    public IEnumerator<T> GetEnumerator()
	{
		for (int i = count - 1; i >= 0; i--)
		{
			yield return items[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
