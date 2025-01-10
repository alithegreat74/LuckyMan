package LuckyManExtensions.utilities;

public class Pair <K,V>{
    public K key;
    public V value;
    public Pair(K key, V value) {
        this.key = key;
        this.value = value;
    }
    public String toString(){
        return key.toString() + " = " + value.toString();
    }
    public boolean equals(Pair other){
        return key.equals(other.key) && value.equals(other.value);
    }
}
