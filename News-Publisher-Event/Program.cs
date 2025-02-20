using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Publisher_Event
{
    public class NewsArticle
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public NewsArticle(int id, string title, string content)
        {
            ID = id;
            Title = title;
            Content = content;
        }

    }

    public class NewsPublisher
    {
        public event EventHandler<NewsArticle> NewsArticlePublished;

        public void PublishNewArticle(int ID, string Title, string Content)
        {
            NewsArticle Article = new NewsArticle(ID, Title, Content);
            OnNewsArticlePublished(Article);
        }

        protected virtual void OnNewsArticlePublished(NewsArticle article)
        {
            NewsArticlePublished?.Invoke(this, article);
        }

    }

    public class NewsSubscriber
    {
        public string SubscriberName { get; }

        public NewsSubscriber(string SubscriberName)
        {
            this.SubscriberName = SubscriberName;
        }

        public void Subscribe(NewsPublisher NewsPublisher)
        {
            NewsPublisher.NewsArticlePublished += NewsPublisher_NewsArticlePublished;
        }
        public void Unsubscribe(NewsPublisher NewsPublisher)
        {
            NewsPublisher.NewsArticlePublished -= NewsPublisher_NewsArticlePublished;
        }

        private void NewsPublisher_NewsArticlePublished(object sender, NewsArticle article)
        {
            Console.WriteLine($"{this.SubscriberName} Received New Article");
            Console.WriteLine($"ID = {article.ID}");
            Console.WriteLine($"Title = {article.Title}");
            Console.WriteLine($"Content = {article.Content}\n\n\n");

        }
        
    }

    internal class Program
    {
        static void Main()
        {
            NewsPublisher publisher = new NewsPublisher();

            // Create subscribers
            NewsSubscriber subscriber1 = new NewsSubscriber("Ahmed");
            NewsSubscriber subscriber2 = new NewsSubscriber("Mohammed");
            NewsSubscriber subscriber3 = new NewsSubscriber("Omar");

            // Subscriptions
            subscriber1.Subscribe(publisher);
            subscriber2.Subscribe(publisher);

            // Publish first article
            Console.WriteLine("Publishing News1...");
            publisher.PublishNewArticle(1, "News1", "alallalalalalala");

            // New subscriber joins
            subscriber3.Subscribe(publisher);

            // Publish second article
            Console.WriteLine("Publishing News2...");
            publisher.PublishNewArticle(2, "News2", "vovovovovovo");

            // Unsubscribing some users
            subscriber2.Unsubscribe(publisher);
            subscriber3.Unsubscribe(publisher);

            // Publish third article
            Console.WriteLine("Publishing News3...");
            publisher.PublishNewArticle(3, "News3", "kkkkkkkppppppp");

            Console.Read();
        }
    }
}
