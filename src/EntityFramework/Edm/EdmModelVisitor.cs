// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Edm
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Linq;

    internal abstract class EdmModelVisitor
    {
        protected static void VisitCollection<T>(IEnumerable<T> collection, Action<T> visitMethod)
        {
            if (collection != null)
            {
                foreach (var element in collection)
                {
                    visitMethod(element);
                }
            }
        }

        protected virtual void VisitAnnotations(MetadataItem item, IEnumerable<DataModelAnnotation> annotations)
        {
            VisitCollection(annotations, VisitAnnotation);
        }

        protected virtual void VisitAnnotation(DataModelAnnotation item)
        {
        }

        protected virtual void VisitMetadataItem(MetadataItem item)
        {
            if (item != null)
            {
                if (item.Annotations.Any())
                {
                    VisitAnnotations(item, item.Annotations);
                }
            }
        }

        public virtual void VisitEntityContainers(IEnumerable<EntityContainer> entityContainers)
        {
            VisitCollection(entityContainers, VisitEdmEntityContainer);
        }

        protected virtual void VisitEdmEntityContainer(EntityContainer item)
        {
            VisitMetadataItem(item);
            if (item != null)
            {
                if (item.EntitySets.Any())
                {
                    VisitEntitySets(item, item.EntitySets);
                }

                if (item.AssociationSets.Any())
                {
                    VisitAssociationSets(item, item.AssociationSets);
                }
            }
        }

        protected virtual void VisitEntitySets(EntityContainer container, IEnumerable<EntitySet> entitySets)
        {
            VisitCollection(entitySets, VisitEdmEntitySet);
        }

        public virtual void VisitEdmEntitySet(EntitySet item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitAssociationSets(
            EntityContainer container, IEnumerable<AssociationSet> associationSets)
        {
            VisitCollection(associationSets, VisitEdmAssociationSet);
        }

        public virtual void VisitEdmAssociationSet(AssociationSet item)
        {
            VisitMetadataItem(item);
            if (item.SourceSet != null)
            {
                VisitEdmAssociationSetEnd(item.SourceSet);
            }
            if (item.TargetSet != null)
            {
                VisitEdmAssociationSetEnd(item.TargetSet);
            }
        }

        protected virtual void VisitEdmAssociationSetEnd(EntitySet item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitComplexTypes(IEnumerable<ComplexType> complexTypes)
        {
            VisitCollection(complexTypes, VisitComplexType);
        }

        protected virtual void VisitComplexType(ComplexType item)
        {
            VisitMetadataItem(item);
            if (item.Properties.Any())
            {
                VisitCollection(item.Properties, VisitEdmProperty);
            }
        }

        protected virtual void VisitDeclaredProperties(ComplexType complexType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitEntityTypes(IEnumerable<EntityType> entityTypes)
        {
            VisitCollection(entityTypes, VisitEdmEntityType);
        }

        protected virtual void VisitEnumTypes(IEnumerable<EnumType> enumTypes)
        {
            VisitCollection(enumTypes, VisitEdmEnumType);
        }

        protected virtual void VisitEdmEnumType(EnumType item)
        {
            VisitMetadataItem(item);
            if (item != null)
            {
                if (item.Members.Any())
                {
                    VisitEnumMembers(item, item.Members);
                }
            }
        }

        protected virtual void VisitEnumMembers(EnumType enumType, IEnumerable<EnumMember> members)
        {
            VisitCollection(members, VisitEdmEnumTypeMember);
        }

        public virtual void VisitEdmEntityType(EntityType item)
        {
            VisitMetadataItem(item);
            if (item != null)
            {
                if (item.DeclaredKeyProperties.Any())
                {
                    VisitDeclaredKeyProperties(item, item.DeclaredKeyProperties);
                }

                if (item.DeclaredProperties.Any())
                {
                    VisitDeclaredProperties(item, item.DeclaredProperties);
                }

                if (item.DeclaredNavigationProperties.Any())
                {
                    VisitDeclaredNavigationProperties(item, item.DeclaredNavigationProperties);
                }
            }
        }

        protected virtual void VisitDeclaredKeyProperties(EntityType entityType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitDeclaredProperties(EntityType entityType, IEnumerable<EdmProperty> properties)
        {
            VisitCollection(properties, VisitEdmProperty);
        }

        protected virtual void VisitDeclaredNavigationProperties(
            EntityType entityType, IEnumerable<NavigationProperty> navigationProperties)
        {
            VisitCollection(navigationProperties, VisitEdmNavigationProperty);
        }

        protected virtual void VisitAssociationTypes(IEnumerable<AssociationType> associationTypes)
        {
            VisitCollection(associationTypes, VisitEdmAssociationType);
        }

        public virtual void VisitEdmAssociationType(AssociationType item)
        {
            VisitMetadataItem(item);

            if (item != null)
            {
                if (item.SourceEnd != null)
                {
                    VisitEdmAssociationEnd(item.SourceEnd);
                }
                if (item.TargetEnd != null)
                {
                    VisitEdmAssociationEnd(item.TargetEnd);
                }
            }
            if (item.Constraint != null)
            {
                VisitEdmAssociationConstraint(item.Constraint);
            }
        }

        protected virtual void VisitEdmProperty(EdmProperty item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitEdmEnumTypeMember(EnumMember item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitEdmAssociationEnd(RelationshipEndMember item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitEdmAssociationConstraint(ReferentialConstraint item)
        {
            if (item != null)
            {
                VisitMetadataItem(item);
                if (item.ToRole != null)
                {
                    VisitEdmAssociationEnd(item.ToRole);
                }
                VisitCollection(item.ToProperties, VisitEdmProperty);
            }
        }

        protected virtual void VisitEdmNavigationProperty(NavigationProperty item)
        {
            VisitMetadataItem(item);
        }

        protected virtual void VisitEdmModel(EdmModel item)
        {
            if (item != null)
            {
                VisitComplexTypes(item.ComplexTypes);
                VisitEntityTypes(item.EntityTypes);
                VisitEnumTypes(item.EnumTypes);
                VisitAssociationTypes(item.AssociationTypes);
                VisitEntityContainers(item.Containers);
            }
        }
    }
}