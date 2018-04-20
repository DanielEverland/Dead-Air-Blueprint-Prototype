using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions {

    public static T Random<T>(this IEnumerable<T> collection)
    {
        return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
    }
    public static void AlignRatio(this Image image)
    {
        if (image.sprite == null)
            return;

        AlignRatio(image.rectTransform, new Vector2(image.sprite.rect.width, image.sprite.rect.height));
    }
    public static void AlignRatio(this RawImage image)
    {
        if (image.texture == null)
            return;

        AlignRatio(image.rectTransform, new Vector2(image.texture.width, image.texture.height));
    }
    private static void AlignRatio(RectTransform transform, Vector2 size)
    {
        Rect rect = GetWorldRect(transform);

        if(size.x > size.y)
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.width * (size.y / size.x));
        }
        else
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.height * (size.x / size.y));
        }
    }
    public static Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        return new Rect(corners[0], rectTransform.rect.size);
    }
    public static T First<T>(this IEnumerable collection)
    {
        foreach (object obj in collection)
        {
            if (obj.GetType() == typeof(T))
                return (T)obj;
        }

        throw new System.NullReferenceException();
    }
}
