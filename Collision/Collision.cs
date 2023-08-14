
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collision
{
	public static void CreateContacts(List<Body> bodies, out List<Contact> contacts)
	{
		contacts = new List<Contact>();
		for (int i = 0; i < bodies.Count - 1; i++)
		{
			for (int j = i + 1; j < bodies.Count; j++)
			{
				Body bodyA = bodies[i];
				Body bodyB = bodies[j];

				if (bodyA.BodyType == Body.eBodyType.DYNAMIC || bodyB.BodyType == Body.eBodyType.DYNAMIC)
				{
					if (TestOverlap(bodyA, bodyB))
					{
						contacts.Add(GenerateContact(bodyA, bodyB));
					}
				}
			}
		}
	}

	public static bool TestOverlap(Body bodyA, Body bodyB)
	{
		return Circle.Intersects(new Circle(bodyA), new Circle(bodyB));
	}

	public static Contact GenerateContact(Body bodyA, Body bodyB)
	{
		Contact contact = new Contact();

		contact.bodyA = bodyA;
		contact.bodyB = bodyB;

		// Compute depth
		Vector2 direction = bodyA.Position - bodyB.Position;
		float distance = direction.magnitude;
		float radius = ((CircleShape)bodyA.shape).Radius + ((CircleShape)bodyB.shape).Radius;
		contact.depth = radius - distance;

		contact.normal = direction.normalized;

		//Vector2 position = bodyB.Position + ((CircleShape)bodyB.shape).Radius * contact.normal;
		//Debug.DrawRay(position, contact.normal);

		return contact;
	}

	public static void CreateBroadPhaseContacts(BroadPhase broadPhase, List<Body> bodies, List<Contact> contacts)
	{
		List<Body> results = new List<Body>();
		for (int i = 0; i < bodies.Count; i++)
		{
			results.Clear();
			// query the broad-phase for potential contacting bodies
			broadPhase.Query(bodies[i], results);



			// add broad-phase contacts 
			for (int j = 0; j < results.Count; j++)
			{
				// check if the result is self and that one of the bodies is a dynamic body
				if (results[j] != bodies[i] &&
				   (results[j].BodyType == Body.eBodyType.DYNAMIC || bodies[i].BodyType == Body.eBodyType.DYNAMIC))
				{
					// check if contact already exists between these two bodies
					//if (!Contact.ExistsIn(contacts, bodies[i], results[j]))
					{
						// create new contact and add to contacts
						Contact contact = new Contact() { bodyA = bodies[i], bodyB = results[j] };
						contacts.Add(contact);
					}
				}
			}
		}

		// Remove duplicate contacts
		contacts.Distinct(new Contact.ItemEqualityComparer());
	}

	public static Contact GenerateContactInfo(Contact contact)
	{
		// compute depth
		Vector2 direction = contact.bodyA.Position - contact.bodyB.Position;
		float distance = direction.magnitude;
		float radius = ((CircleShape)contact.bodyA.shape).Radius + ((CircleShape)contact.bodyB.shape).Radius;
		contact.depth = radius - distance;

		// compute normal
		contact.normal = direction.normalized;

		return contact;
	}

	public static void CreateNarrowPhaseContacts(List<Contact> contacts)
	{
		// remove contacts from narrow-phase test
		contacts.RemoveAll(contact => (TestOverlap(contact.bodyA, contact.bodyB) == false));
		// generate contact info from remaining contacts
		for (int i = 0; i < contacts.Count; i++)
		{
			GenerateContactInfo(contacts[i]);
		}
	}

	public static void SeparateContacts(List<Contact> contacts)
	{
		foreach (Contact contact in contacts)
		{
			float totalInverseMass = contact.bodyA.InverseMass + contact.bodyB.InverseMass;

			Vector2 separation = contact.normal * contact.depth / totalInverseMass;
			contact.bodyA.Position += separation * contact.bodyA.InverseMass;
			contact.bodyB.Position -= separation * contact.bodyB.InverseMass;
		}

	}

	public static void ApplyImpulses(List<Contact> contacts)
	{
		foreach (Contact contact in contacts)
		{
			Vector2 relativeVelocity = contact.bodyA.Velocity - contact.bodyB.Velocity;
			float normalVelocity = Vector2.Dot(relativeVelocity, contact.normal);

			if (normalVelocity > 0.0f) continue;

			float totalInverseMass = contact.bodyA.InverseMass + contact.bodyB.InverseMass;

			float restitution = (contact.bodyA.restitution + contact.bodyB.restitution) * 0.5f;

			float impulseMagnitude = -((1 + restitution) * normalVelocity) / totalInverseMass;

			Vector2 impulse = contact.normal * impulseMagnitude;

			contact.bodyA.ApplyForce(contact.bodyA.Velocity + (impulse * contact.bodyA.InverseMass), Body.ForceMode.VELOCITY);
			contact.bodyB.ApplyForce(contact.bodyB.Velocity + (-impulse * contact.bodyB.InverseMass), Body.ForceMode.VELOCITY);
		}
	}
}
